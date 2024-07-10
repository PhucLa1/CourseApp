using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class UserExercise : BaseEntities
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("exercise_id")]
        public int ExerciseId { get; set; }
        [Column("content_code")]
        public required string ContentCode { get; set; }
        [Column("is_success")]
        public bool IsSuccess { get; set; }
        public User? User { get; set; }
        public Exercise? Exercise { get; set; }
    }
}
