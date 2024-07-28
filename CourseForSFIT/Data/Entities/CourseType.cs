using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class CourseType : BaseEntities
    {
        [Column("name")]
        public required string Name { get; set; }
        public ICollection<Course>? Courses { get; set; }
    }
}
