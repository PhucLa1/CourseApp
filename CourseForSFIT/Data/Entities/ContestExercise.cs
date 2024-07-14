using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ContestExercise : BaseEntities
    {
        [Column("exercise_id")]
        public int ExerciseId { get; set; }
        [Column("contest_id")]
        public int ContestId { get; set; }

    }
}
