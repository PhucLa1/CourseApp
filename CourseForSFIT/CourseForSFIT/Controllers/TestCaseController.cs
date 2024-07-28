using Dtos.Models.ExerciseModels;
using Dtos.Models.TestCaseModels;
using Microsoft.AspNetCore.Mvc;
using Services.TestCases;

namespace CourseForSFIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestCaseController : ControllerBase
    {
        private readonly ITestCaseService _testCaseService;
        private readonly ISolveTestCaseService _solveTestCaseService;
        public TestCaseController(ITestCaseService testCaseService, ISolveTestCaseService solveTestCaseService)
        {
            _testCaseService = testCaseService;
            _solveTestCaseService = solveTestCaseService;
        }
        [HttpGet]
        [Route("get-test-cases-not-lock/{exerciseId}")]
        public async Task<IActionResult> GetAllTestCaseNotLockByExerciseId(int exerciseId)
        {
            return Ok(await _testCaseService.GetAllTestCaseNotLockByExerciseId(exerciseId));
        }
        [HttpGet]
        [Route("get-test-cases/{exerciseId}")]
        public async Task<IActionResult> GetAllTestCaseByExerciseId(int exerciseId)
        {
            return Ok(await _testCaseService.GetAllTestCaseByExerciseId(exerciseId));
        }
 
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTestCase(int id)
        {
            return Ok(await _testCaseService.DeleteTestCase(id));
        }


        [HttpPost]
        [Route("solve-test-case")]
        public async Task<IActionResult> SolveTestCase([FromBody] TestCaseSolve testCaseSolve)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _solveTestCaseService.SolveExerciseProblem(testCaseSolve));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTestCase(int id, [FromForm] TestCaseExerciseUpdateDto testCaseExerciseUpdateDto)
        {
            return Ok(await _testCaseService.UpdateTestCase(id, testCaseExerciseUpdateDto));
        }
    }
}
