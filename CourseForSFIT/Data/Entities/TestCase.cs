using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class TestCase : BaseEntities
    {
        [Column("input_data")]
        public string? InputData { get; set; }
        [Column("exercise_id")]
        public int ExerciseId { get; set; }
        [Column("expected_output")]
        public required string ExpectedOutput { get; set; }
    }
}
