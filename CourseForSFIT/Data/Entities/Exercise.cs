using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Exercise : BaseEntities
    {
        [Column("exercise_name")]
        public required string ExerciseName { get; set; }
        [Column("difficult_level")]
        public int DifficultLevel { get; set; }
        [Column("content_exercise")]
        public required string ContentExercise { get; set; }
        [Column("number_participants")]
        public int NumberParticipants { get; set; }
        [Column("success_rate")]
        public double SuccessRate { get; set; }
        public ICollection<ExerciseHasTag>? ExerciseHasTags { get; set; }
        public ICollection<ExerciseComment>? ExerciseComments { get; set; }
        public ICollection<ContestExercise>? ContestExercises { get; set; }
        public ICollection<UserExercise>? UserExercises { get; set; }
        public ICollection<TestCase>? TestCases { get; set; }
    }
}
