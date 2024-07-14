using Dtos.Models.AuthModels;
using Dtos.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Users;

namespace CourseForSFIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        [Route("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto userSignUp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApiResponse<string> result = await _authService.SignUp(userSignUp);
            if (result.IsSuccess)
            {
                SetJWT(result.Metadata);
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApiResponse<string> result = await _authService.Login(userLoginDto);
            if (result.IsSuccess)
            {
                SetJWT(result.Metadata);
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("login-google")]
        public async Task<IActionResult> LoginGoogle(string credential)
        {
            ApiResponse<string> result = await _authService.GoogleLogin(credential);
            if (result.IsSuccess)
            {
                SetJWT(result.Metadata);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("login-facebook")]
        public async Task<IActionResult> LoginFacebook(string credential)
        {
            try
            {
                ApiResponse<string> result = await _authService.FacebookLogin(credential);
                if (result.IsSuccess)
                {
                    SetJWT(result.Metadata);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpDelete]
        [Route("log-out")]
        public async Task<ActionResult> SignOut()
        {
            try
            {
                DeleteJWT();
                return Ok(new ApiResponse<bool> { IsSuccess = true });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        [Route("generate-code")]
        public async Task<IActionResult> GenerateVerificationCode([FromBody] string email)
        {
            try
            {
                return Ok(await _authService.GenerateVerificationCode(email));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        [Route("verify-code")]
        public async Task<ActionResult> VerifyVerificationCode([FromBody] VerifyVerificationCodeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _authService.VerifyVerificationCode(request));
        }
        [HttpPost]
        [Route("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _authService.UpdatePassword(resetPassword));
        }
        [HttpGet]
        [Route("get-current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            return Ok(await _authService.GetCurrentUser());
        }
        private void SetJWT(string encryptedToken)
        {
            HttpContext.Response.Cookies.Append("X-Access-Token", encryptedToken,
                 new CookieOptions
                 {
                     Expires = DateTime.UtcNow.AddDays(15),
                     HttpOnly = true,
                     Secure = true,
                     IsEssential = true,
                     SameSite = SameSiteMode.None
                 });
        }

        private void DeleteJWT()
        {
            HttpContext.Response.Cookies.Delete("X-Access-Token");
        }
    }
}
