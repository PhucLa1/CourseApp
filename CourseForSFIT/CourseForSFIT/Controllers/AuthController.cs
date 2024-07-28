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
            ApiResponse<string> result = await _authService.Login(userLoginDto);
            if (result.Metadata != null)
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
            if (result.Metadata != null)
            {
                SetJWT(result.Metadata);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("login-facebook")]
        public async Task<IActionResult> LoginFacebook(string credential)
        {
            ApiResponse<string> result = await _authService.FacebookLogin(credential);
            if (result.Metadata != null)
            {
                SetJWT(result.Metadata);
            }
            return Ok(result);
        }
        [HttpDelete]
        [Route("log-out")]
        public async Task<ActionResult> SignOut()
        {
            DeleteJWT();
            return Ok(new ApiResponse<bool> { IsSuccess = true });
        }
        [HttpPost]
        [Route("generate-code")]
        public async Task<IActionResult> GenerateVerificationCode([FromBody] string email)
        {
            return Ok(await _authService.GenerateVerificationCode(email));
        }
        [HttpPost]
        [Route("verify-code")]
        public async Task<ActionResult> VerifyVerificationCode([FromBody] VerifyVerificationCodeRequest request)
        {
            return Ok(await _authService.VerifyVerificationCode(request));
        }
        [HttpPost]
        [Route("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ResetPassword resetPassword)
        {
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
