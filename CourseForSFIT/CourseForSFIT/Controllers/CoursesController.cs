using Dtos.Models.CourseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Courses;

namespace Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddNewCourse([FromForm] CourseAddDto courseAddDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _courseService.AddNewCourse(courseAddDto));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromForm] CourseUpdateDto courseUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _courseService.UpdateCourse(id, courseUpdateDto));
        }

        [HttpPost]
        [Route("get-course-by-options")]
        public async Task<IActionResult> GetCourseByOptions(CourseRequest courseRequest)
        {
            return Ok(await _courseService.GetCourseByOptionsAdminPage(courseRequest));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            return Ok(await _courseService.DeleteCourse(id));
        }
    }
}
