using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results.ExerciseResults
{
    public class ExerciseAdminDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int DifficultLevel { get; set; }
        public int NumberParticipants { get; set; }
        public double SuccessRate { get; set; }
        public List<string>? Tags { get; set; }
    }
}
