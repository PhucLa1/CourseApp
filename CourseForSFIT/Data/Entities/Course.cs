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
        [Column("course_type_id")]
        public int CourseTypeId { get; set; }
    }
}
