using Dtos.Models.TestCaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories.IRepo;
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
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddTestCase([FromBody]TestCaseAddDto testCaseAddDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _testCaseService.AddTestCase(testCaseAddDto));
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTestCase(int id)
        {
            return Ok(await _testCaseService.DeleteTestCase(id));
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTestCase(int id, [FromBody]TestCaseUpdateDto testCaseUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _testCaseService.UpdateTestCase(id, testCaseUpdateDto));
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
    }
}
