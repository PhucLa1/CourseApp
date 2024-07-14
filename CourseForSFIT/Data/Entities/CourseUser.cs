using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class CourseUser : BaseEntities
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("course_id")]
        public int CourseId { get; set; }
        [Column("complete")]
        public double Complete { get; set; }

    }
}
