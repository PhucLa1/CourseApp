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
        [Column("name")]
        public required string Name { get; set; }
        [Column("difficult_level")]
        public int DifficultLevel { get; set; }
        [Column("content")]
        public required string Content { get; set; }
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
