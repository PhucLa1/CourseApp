using Dtos;
using Dtos.Models.ExerciseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Exercises;
using Shared;

namespace CourseForSFIT.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ExercisesController : ControllerBase
    {
        
        private readonly IExerciseService _exerciseService;

        public ExercisesController(ITagExerciseService tagExerciseService, IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpPost]
        [Route("get-exercises-by-options")]
        public async Task<IActionResult> GetPaginatedExercisesByOptions([FromBody]ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10)
        {
            return Ok(await _exerciseService.GetExerciseByOptionsPaginated(exerciseRequest,pageNumber, pageSize));
        }
        [HttpPost]
        [Route("get-admin-exercises-by-options")]
        public async Task<IActionResult> GetAdminPaginatedExercisesByOptions([FromBody] ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10)
        {
            return Ok(await _exerciseService.GetAdminExerciseByOptionsPaginated(exerciseRequest, pageNumber, pageSize));
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTopicExerciseById(int id)
        {
            return Ok(await _exerciseService.GetTopicExercise(id));
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            return Ok(await _exerciseService.DeleteExercise(id));
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddExercise([FromForm]ExerciseAddDto exerciseAddDto)
        {
            return Ok(await _exerciseService.AddExercise(exerciseAddDto));
        }
    }
}
