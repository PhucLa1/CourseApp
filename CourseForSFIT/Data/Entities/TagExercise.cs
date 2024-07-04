using Data.Entities.Base;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class TagExercise : BaseEntities
    {
        [Column("tag_name")]
        [Unique]
        public required string TagName { get; set; }
    }
}
