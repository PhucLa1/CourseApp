using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results.ExerciseResults
{
    public class UserExerciseSubmit
    {
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? SuccessRate { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
