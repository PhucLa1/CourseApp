using Dtos;
using Dtos.Models.ExerciseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Exercises;
using Services.TestCases;
using Shared;

namespace CourseForSFIT.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ExercisesController : ControllerBase
    {

        private readonly IExerciseService _exerciseService;
        private readonly IUserExerciseService _userExerciseService;
        private readonly ITestCaseService _testCaseService;

        public ExercisesController(IExerciseService exerciseService, IUserExerciseService userExerciseService, ITestCaseService testCaseService)
        {
            _exerciseService = exerciseService;
            _userExerciseService = userExerciseService;
            _testCaseService = testCaseService;
        }

        [HttpPost]
        [Route("get-exercises-by-options")]
        public async Task<IActionResult> GetPaginatedExercisesByOptions([FromBody] ExerciseRequest exerciseRequest, int pageNumber = 1, int pageSize = 10)
        {
            return Ok(await _exerciseService.GetExerciseByOptionsPaginated(exerciseRequest, pageNumber, pageSize));
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
        public async Task<IActionResult> AddExercise([FromForm] ExerciseAddDto exerciseAddDto)
        {
            return Ok(await _exerciseService.AddExercise(exerciseAddDto));
        }
        [HttpGet]
        [Route("get-content-code/{exerciseId}")]
        public async Task<IActionResult> GetContentCode(int exerciseId)
        {
            return Ok(await _userExerciseService.GetContentCodeLastest(exerciseId));
        }
        [HttpGet]
        [Route("get-user-submission/{exerciseId}")]
        public async Task<IActionResult> GetUserSubmission(int exerciseId, int isMine = 1, int pageNumber = 1, int pageSize = 10)
        {
            return Ok(await _userExerciseService.GetUserSubmission(exerciseId, isMine, pageNumber, pageSize));
        }
        [HttpGet]
        [Route("get-exercise-info-admin/{id}")]
        public async Task<IActionResult> GetExerciseInfo(int id)
        {
            return Ok(await _exerciseService.GetExerciseInfo(id));
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, [FromBody]ExerciseUpdateDto exerciseUpdateDto)
        {
            return Ok(await _exerciseService.UpdateExercise(id, exerciseUpdateDto));
        }
        [HttpPost]
        [Route("{exerciseId}/test-cases")]
        public async Task<IActionResult> AddTestCase(int exerciseId, [FromForm] TestCaseExerciseAddDto testCaseExerciseAddDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _testCaseService.AddTestCase(exerciseId, testCaseExerciseAddDto));
        }
    }
}
