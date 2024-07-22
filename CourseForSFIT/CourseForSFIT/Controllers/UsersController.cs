using Dtos.Models.UserModels;
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
        [Route("get-current-user-info")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            return Ok(await _userService.GetUserInfo());
        }
        [HttpPut]
        [Route("update-current-user")]
        public async Task<IActionResult> UpdateUserInfo([FromForm]UserUpdateDto userUpdateDto)
        {
            return Ok(await _userService.UpdateUserInfo(userUpdateDto));
        }
        [HttpPut]
        [Route("update-achievements")]
        public async Task<IActionResult> UpdateAchievements([FromBody] List<string>? achievements)
        {
            return Ok(await _userService.UpdateAchievement(achievements));
        }
    }
}
