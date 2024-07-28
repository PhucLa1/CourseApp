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
    public class ChaptersController : ControllerBase
    {
        private readonly IChapterService _chapterService;
        public ChaptersController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }
        [HttpGet]
        [Route("get-by-course-id/{courseId}")]
        public async Task<IActionResult> GetChaptersByCourseId(int courseId)
        {
            return Ok(await _chapterService.GetAllChaptesByCourseId(courseId));
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddNewChapter(ChapterAdd chapterAdd)
        {
            return Ok(await _chapterService.AddNewChapter(chapterAdd));
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateChapter(int id, [FromBody]string chapterName)
        {
            return Ok(await _chapterService.UpdateChapter(id, chapterName));
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            return Ok(await _chapterService.DeleteChapter(id));
        }

    }
}
