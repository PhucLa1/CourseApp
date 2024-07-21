using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Users;

namespace Apis.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) 
        { 
            _userService = userService;
        }
        [HttpGet]
        [Route("get-current-user")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            return Ok(await _userService.GetUserInfo());
        }
    }
}
