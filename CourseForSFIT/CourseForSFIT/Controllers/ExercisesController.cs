using Dtos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Exercises;

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
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTopicExerciseById(int id)
        {
            return Ok(await _exerciseService.GetTopicExercise(id));
        }
        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            return Ok(await _exerciseService.Test());
        }
    }
}
