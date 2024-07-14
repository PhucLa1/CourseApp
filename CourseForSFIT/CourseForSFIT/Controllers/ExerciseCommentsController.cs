using Dtos.Models.ExerciseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Exercises;

namespace CourseForSFIT.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ExerciseCommentsController : ControllerBase
    {
        private readonly IExerciseCommentService _exerciseCommentService;
        public ExerciseCommentsController(IExerciseCommentService exerciseCommentService)
        {
            _exerciseCommentService = exerciseCommentService;
        }
        [HttpGet]
        [Route("get-by-exercise-id/{exerciseId}")]
        public async Task<IActionResult> GetCommentByExerciseId( int exerciseId)
        {
            return Ok(await _exerciseCommentService.GetExerciseComment(exerciseId));
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddCommentByExerciseId([FromBody]CommentExerciseAddDto commentAddDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _exerciseCommentService.AddComment(commentAddDto));
        }
    }
}
