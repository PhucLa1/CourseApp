using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Course : BaseEntities
    {
        [Column("course_name")]
        public required string CourseName { get; set; }
        [Column("description")]
        public required string Description { get; set; }
        [Column("learn_about")]
        public string? LearnAbout { get; set; }
        [Column("prepared")]
        public string? Prepared { get; set; }
        [Column("thumbnail")]
        public string? Thumbnail { get; set; }
        [Column("status")]
        public int Status { get; set; } //1: Draft, 2: Public
        [Column("course_type_id")]
        public int CourseTypeId { get; set; }
        public ICollection<CourseUser>? CourseUsers { get; set; }
        public ICollection<CourseComment>? CourseComments { get; set; }
        public ICollection<Chapter>? Chapters { get; set; }
    }
}
