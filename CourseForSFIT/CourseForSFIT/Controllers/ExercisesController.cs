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
        private readonly ITagExerciseService _tagExerciseService;
        private readonly IExerciseService _exerciseService;
        public ExercisesController(ITagExerciseService tagExerciseService, IExerciseService exerciseService)
        {
            _tagExerciseService = tagExerciseService;
            _exerciseService = exerciseService;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllTagExercises()
        {
            return Ok(await _tagExerciseService.GetAllTagExercise());
        }
        [HttpGet]
        [Route("get-exercises")]
        public async Task<IActionResult> GetPaginatedExercises(int pageNumber = 1, int pageSize = 10)
        {
            return Ok(await _exerciseService.GetExercisesPaginated(pageNumber,pageSize));
        }
    }
}
