using Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class LessonComment : BaseEntities
    {
        [Column("content")]
        public required string Content { get; set; }
        [Column("lesson_id")]
        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }
    }
}
