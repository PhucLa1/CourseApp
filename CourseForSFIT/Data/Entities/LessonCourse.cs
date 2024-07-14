using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class LessonCourse : BaseEntities
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("lesson_id")]
        public int LessonId { get; set; }
        [Column("complete_level")]
        public double CompleLevel { get; set; }

    }
}
