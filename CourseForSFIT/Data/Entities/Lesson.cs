
using Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data.Entities
{
    public class Lesson : BaseEntities
    {
        [Column("name")]
        public required string Name { get; set; }
        [Column("type")]
        public LessonType Type { get; set; } = LessonType.Video;
        [Column("chapter_id")]
        public int ChapterId { get; set; }
        [Column("content_lesson")]
        public string? Content { get; set; }
        [Column("status_upload")]
        public StatusUpload StatusUpload { get; set; } = StatusUpload.Loading;
        [Column("status")]
        public LessonStatus Status { get; set; } = LessonStatus.Private;
        [Column("chunk_index")]
        public int ChunkIndex { get; set; }
        [Column("total_chunk")]
        public int TotalChunk { get; set; }
        public ICollection<LessonCourse>? LessonCourses { get; set; }
        public ICollection<LessonComment>? LessonComments { get; set; }

    }
    public enum StatusUpload
    {
        Loading = 1,
        Success = 2, 
        Failed = 3
    }
    public enum LessonStatus
    {
        Private = 1,
        Public = 2
    }
    public enum LessonType
    {
        Text = 1,
        Video = 2
    }
}
