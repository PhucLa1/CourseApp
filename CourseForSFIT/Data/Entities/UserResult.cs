using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class UserResult : BaseEntities
    {
        [Column("test_case_id")]
        public int TestCaseId { get; set; }
        [Column("user_exercise_id")]
        public int UserExerciseId { get; set; }
        [Column("is_pass")]
        public bool IsPass { get; set; }
    }
}
