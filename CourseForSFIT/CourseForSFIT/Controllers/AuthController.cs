using Dtos.Models;
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
        [HttpPost]
        [Route("gen-sql")]
        public async Task<IActionResult> GenSql()
        {
            List<string> tagNames = new List<string>()
            {
                "Sắp xếp",
                "Tìm kiếm",
                "Đệ quy",
                "Quy hoạch động",
                "Đồ thị",
                "Lý thuyết số",
                "Thuật toán tham lam",
                "Chia để trị",
                "Backtracking",
                "Duyệt cây"
            };
            string sql = "";
            foreach(string tag in tagNames)
            {
                string tagName = tag;
                string createdAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string updateAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int createdBy = 1;
                int updatedBy = 1;
                string sqlGens = string.Format("insert into tag_exercise(tag_name, created_at,updated_at, created_by, updated_by) values('{0}', '{1}', '{2}', {3}, {4})",
            tagName, createdAt, updateAt, createdBy, updatedBy);
                sql += sqlGens + "\n";
            }
            return Ok(sql);
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
