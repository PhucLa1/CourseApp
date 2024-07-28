using Data.Entities.Base;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class TagExercise : BaseEntities
    {
        [Column("name")]
        [Unique]
        public required string Name { get; set; }     
        public ICollection<ExerciseHasTag>? ExerciseHasTags { get; set; }
    }
}
