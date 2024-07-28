using Dtos.Models.CourseModels;
using Microsoft.AspNetCore.Mvc;
using Services.Courses;

namespace Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet]
        [Route("get-lesson-by-chapter-id/{chapterId}")]
        public async Task<IActionResult> GetLessonByChapterId(int chapterId)
        {
            return Ok(await _lessonService.GetLessonByChapterId(chapterId));
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddNewLesson([FromBody] LessonAddDto lessonAdd)
        {
            return Ok(await _lessonService.AddNewLesson(lessonAdd));
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            return Ok(await _lessonService.DeleteLesson(id));
        }

        [HttpPut]
        [Route("upload-chunk-file/{id}")]
        public async Task<IActionResult> UploadChunkFileAfterAddLesson(int id, [FromForm] LessonUploadChunkFile lessonUploadChunkFile)
        {
            return Ok(await _lessonService.UpdateChunkFile(lessonUploadChunkFile.ChunkFile, lessonUploadChunkFile.ChunkIndex, lessonUploadChunkFile.TotalChunk, id));
        }
    }
}
