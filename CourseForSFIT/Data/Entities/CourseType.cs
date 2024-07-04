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
        [Column("type_name")]
        public required string TypeName { get; set; }
    }
}
