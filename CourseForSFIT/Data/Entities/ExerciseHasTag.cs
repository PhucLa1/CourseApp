using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ExerciseHasTag : BaseEntities
    {
        [Column("tag_exercise_id")]
        public int TagExerciseId { get; set; }
        [Column("exercise_id")]
        public int ExerciseId { get; set; }

    }
}
