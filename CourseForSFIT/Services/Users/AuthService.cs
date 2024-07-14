using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Repositories.Repositories.IRepo;
using Repositories.unitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Dtos.Results;
using Google.Apis.Auth;
using Newtonsoft.Json;
using Shared;
using Dtos.Models.AuthModels;
using Dtos.Models.EmailModels;
using Dtos.Results.AuthResults;


namespace Services.Users
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> SignUp(UserSignUpDto userSignup);
        Task<ApiResponse<string>> Login(UserLoginDto userLoginDto);
        Task<ApiResponse<string>> GoogleLogin(string credential);

        Task<ApiResponse<string>> FacebookLogin(string credential);
        Task<ApiResponse<bool>> UpdatePassword(ResetPassword resetPassword);
        Task<ApiResponse<bool>> GenerateVerificationCode(string email);
        Task<ApiResponse<bool>> VerifyVerificationCode(VerifyVerificationCodeRequest verifyVerificationCodeRequest);
        Task<ApiResponse<UserDto>> GetCurrentUser();
    }
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            HttpClient httpClient,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _unitOfWork = unitOfWork;
            userRepository = unitOfWork.UserRepository;
            _mapper = mapper;
            _configuration = configuration;
            _httpClient = httpClient;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<string>> SignUp(UserSignUpDto userSignup)
        {
            try
            {
                User? userEmail = await userRepository.FindUserByEmail(userSignup.Email);
                if (userSignup.RePassword != userSignup.Password)
                {
                    return new ApiResponse<string> { Message = ["Mật khẩu và nhập lại mật khẩu không giống nhau"] };
                }
                if (userEmail != null)
                {
                    return new ApiResponse<string> { Message = ["Tài khoản email này đã tổn tại rồi"] };
                }

                User user = _mapper.Map<User>(userSignup);
                user.Password = BCrypt.Net.BCrypt.HashPassword(userSignup.Password);
                user.Role = 1;
                user.JoinDate = DateTime.UtcNow;
                await userRepository.AddAsync(user);
                await _unitOfWork.SaveAsync();
                User? userAdd = await userRepository.FindUserByEmail(userSignup.Email);
                string token = await JWTGenerator(userAdd);
                return new ApiResponse<string> { Metadata = token, IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<UserDto>> GetCurrentUser()
        {
            try
            {
                int currentUserId = _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
                return new ApiResponse<UserDto> { IsSuccess = true, Metadata = _mapper.Map<UserDto>(await _unitOfWork.UserRepository.GetByIdAsync(currentUserId)) };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<string>> FacebookLogin(string credential)
        {
            try
            {
                string? appId = _configuration["Facebook:AppId"];
                string? appSecret = _configuration["Facebook:AppSecret"];
                HttpResponseMessage debugTokenResponse = await _httpClient.GetAsync($"https://graph.facebook.com/debug_token?input_token={credential}&access_token={appId}|{appSecret}");
                var stringThing = await debugTokenResponse.Content.ReadAsStringAsync();
                var userOBJK = JsonConvert.DeserializeObject<FacebookTokenValidationResult>(stringThing);

                if (!userOBJK.Data.IsValid)
                {
                    return new ApiResponse<string> { Message = ["Dữ liệu đầu vào không chính xác"] };
                }
                HttpResponseMessage meResponse = await _httpClient.GetAsync($"https://graph.facebook.com/me?fields=first_name,last_name,picture,email&access_token={credential}");
                var userContent = await meResponse.Content.ReadAsStringAsync();
                var userContentObj = JsonConvert.DeserializeObject<FacebookUserInfoResult>(userContent);

                User? userEmail = await userRepository.FindUserByEmail(userContentObj.Email);
                User user = _mapper.Map<User>(userContentObj);
                if (userEmail == null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Email + DateTime.UtcNow.ToString());
                    await userRepository.AddAsync(user);
                    await SendEmail(user.Email);
                }
                User? userAdd = await userRepository.FindUserByEmail(user.Email);
                string token = await JWTGenerator(userAdd);
                return new ApiResponse<string> { IsSuccess = true, Metadata = token };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> GoogleLogin(string credential)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string> { _configuration["Google:ClientID"] }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);
                User? userEmail = await userRepository.FindUserByEmail(payload.Email);
                User user = new User()
                {
                    Email = payload.Email,
                    FirstName = payload.FamilyName,
                    LastName = payload.Name,
                    Password = BCrypt.Net.BCrypt.HashPassword(payload.Email + DateTime.UtcNow.ToString()),
                    Role = 1,
                    JoinDate = DateTime.UtcNow,
                    Avatar = payload.Picture
                };
                if (userEmail == null)
                {
                    await userRepository.AddAsync(user);
                    await SendEmail(user.Email);
                }
                User? userAdd = await userRepository.FindUserByEmail(user.Email);
                string token = await JWTGenerator(userAdd);
                return new ApiResponse<string> { Metadata = token, IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private async Task<string> JWTGenerator(User Result)
        {
            try
            {
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Name, _configuration.GetValue<string>("Jwt:Subject")),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                        new Claim("Id", Result.Id.ToString()),
                        new Claim("Email", Result.Email),
                        new Claim("FirstName", Result.FirstName),
                        new Claim("LastName", Result.LastName),
                        new Claim("Role",Result.Role.ToString())
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration.GetValue<string>("Jwt:Issuer"),
                    _configuration.GetValue<string>("Jwt:Audience"),
                    claims,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: signIn);

                var encrypterToken = new JwtSecurityTokenHandler().WriteToken(token);
                return encrypterToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task SendEmail(String email)
        {
            Email emailSend = new Email()
            {
                To = email,
                Subject = "Sign up successfully",
                Body = "Sign up successfully",
            };
            await _emailSender.SendEmail(emailSend);
        }

        public async Task<ApiResponse<string>> Login(UserLoginDto userLoginDto)
        {
            try
            {
                User? user = await userRepository.FindUserByEmail(userLoginDto.Email);
                if (user == null)
                {
                    return new ApiResponse<string> { Message = ["Email không tồn tại"] };
                }
                bool isCorrectPass = BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password);
                if (!isCorrectPass)
                {
                    return new ApiResponse<string> { Message = ["Mật khẩu không đúng"] };
                }
                string encrypterToken = await JWTGenerator(user);
                return new ApiResponse<string> { Metadata = encrypterToken, IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<ApiResponse<bool>> UpdatePassword(ResetPassword resetPassword)
        {
            try
            {
                if (resetPassword.Password != resetPassword.RePassword)
                {
                    return new ApiResponse<bool> { Message = ["Mật khẩu và mật khẩu nhập lại không đúng"] };
                }
                await userRepository.ChangePassword(resetPassword);
                await _unitOfWork.SaveAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<ApiResponse<bool>> GenerateVerificationCode(string email)
        {
            try
            {
                string code = HandleString.GenerateRandomCode();

                VerifyVerificationCodeRequest verifyVerificationCodeRequest = new VerifyVerificationCodeRequest()
                {
                    Email = email,
                    Code = code
                };
                bool isSendEmail = await userRepository.UpdateCodeAndTimeSend(verifyVerificationCodeRequest);
                if (isSendEmail)
                {
                    Email emailSend = new Email()
                    {
                        To = email,
                        Subject = "Verify code",
                        Body = verifyVerificationCodeRequest.Code,
                    };
                    await _emailSender.SendEmail(emailSend);
                    await _unitOfWork.SaveAsync();
                    return new ApiResponse<bool> { IsSuccess = true };
                }

                return new ApiResponse<bool> { Message = ["Không tồn tại email nào như thế cả"] };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> VerifyVerificationCode(VerifyVerificationCodeRequest verifyVerificationCodeRequest)
        {
            try
            {
                User? userEmail = await userRepository.FindUserByEmail(verifyVerificationCodeRequest.Email);
                if (userEmail.Code != verifyVerificationCodeRequest.Code)
                {
                    return new ApiResponse<bool> { Message = ["Mã code được gửi đi bị sai"] };
                }
                else if (userEmail.ExpiredTime < DateTime.UtcNow)
                {
                    return new ApiResponse<bool> { Message = ["Mã code đã hết hạn"] };
                }
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}