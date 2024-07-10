using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Chapter : BaseEntities
    {
        [Column("chapter_name")]
        public required string ChapterName { get; set; }
        [Column("course_id")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public ICollection<Lesson>? Lessons { get; set; }
    }
}
