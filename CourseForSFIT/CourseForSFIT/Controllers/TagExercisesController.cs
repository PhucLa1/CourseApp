using Dtos.Models.ExerciseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Exercises;

namespace CourseForSFIT.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TagExercisesController : ControllerBase
    {
        private readonly ITagExerciseService _tagExerciseService;
        public TagExercisesController(ITagExerciseService tagExerciseService)
        {
            _tagExerciseService = tagExerciseService;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllTagExercises()
        {
            return Ok(await _tagExerciseService.GetAllTagExercise());
        }
        [HttpGet]
        [Route("admin")]
        [Authorize(Policy = "Role")]
        public async Task<IActionResult> GetAllAdminTagExercises()
        {
            return Ok(await _tagExerciseService.GetAllTagExerciseAdminDto());
        }
        [HttpGet]
        [Route("admin/{id}")]
        [Authorize(Policy = "Role")]
        public async Task<IActionResult> GetAdminTagExercisesById(int id)
        {
            return Ok(await _tagExerciseService.GetTagExercisesById(id));
        }
        [HttpPut]
        [Route("admin/{id}")]
        [Authorize(Policy = "Role")]
        public async Task<IActionResult> UpdateAdminTagExercisesById(int id, TagExerciseUpdateDto tagExerciseUpdateDto)
        {
            return Ok(await _tagExerciseService.UpdateTagExercise(id, tagExerciseUpdateDto));
        }
        [HttpDelete]
        [Route("admin/{id}")]
        [Authorize(Policy = "Role")]
        public async Task<IActionResult> DeleteAdminTagExercisesById(int id)
        {
            return Ok(await _tagExerciseService.DeleteTagExercise(id));
        }
        [HttpPost]
        [Route("admin")]
        [Authorize(Policy = "Role")]
        public async Task<IActionResult> DeleteAdminTagExercisesById(TagExerciseAddDto tagExerciseAddDto)
        {
            return Ok(await _tagExerciseService.AddTagExercise(tagExerciseAddDto));
        }
        [HttpPost]
        [Route("add-data-sample")]
        public async Task<IActionResult> AddDataSample()
        {
            return Ok(await _tagExerciseService.AddDataSample());
        }
        [HttpGet]
        [Route("get-tags-exericse-by-exercise-id/{exerciseId}")]
        public async Task<IActionResult> GetTagExercisesByExerciseId(int exerciseId)
        {
            return Ok(await _tagExerciseService.GetTagExercisesByExerciseId(exerciseId));
        }
    }
}
