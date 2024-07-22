using Dtos.Models.CourseTypeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Courses;

namespace Apis.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CourseTypesController : ControllerBase
    {
        private readonly ICourseTypeService _courseTypeService;
        public CourseTypesController(ICourseTypeService courseTypeService) { _courseTypeService = courseTypeService; }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllCourseType()
        {
            return Ok(await _courseTypeService.GetAllCourseTypes());
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddNewCourseType([FromBody]CourseTypeAdd courseTypeAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _courseTypeService.AddNewCourseType(courseTypeAdd));
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UdpdateCourseType(int id, [FromBody] CourseTypeUpdate courseTypeUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _courseTypeService.UpdateCourseType(id , courseTypeUpdate));
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCourseType(int id)
        {
            return Ok(await _courseTypeService.DeleteCourseType(id));
        }
    }
}
