
using Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data.Entities
{
    public class Lesson : BaseEntities
    {
        [Column("lesson_name")]
        public required string LessonName { get; set; }
        [Column("type")]
        public int Type { get; set; }
        [Column("is_lock")]
        public bool IsLock { get; set; }
        [Column("chapter_id")]
        public int ChapterId { get; set; }
        public ICollection<LessonCourse>? LessonCourses { get; set; }
        public ICollection<LessonComment>? LessonComments { get; set; }

    }
}
