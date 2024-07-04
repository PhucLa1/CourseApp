using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string? ExerciseName { get; set; }
        public int DifficultLevel { get; set; }
        public int NumberParticipants { get; set; }
        public double SuccessRate { get; set; }
    }
}
