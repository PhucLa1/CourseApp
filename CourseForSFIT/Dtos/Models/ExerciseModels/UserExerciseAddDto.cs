using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.ExerciseModels
{
    public class UserExerciseAddDto
    {
        public int UserId { get; set; }
        public int ExerciseId { get; set; }
        public required string ContentCode { get; set; }
        public string? SuccessRate { get; set; }
        public bool IsSuccess { get; set; }
        public string? Avatar { get; set; }
        public string Language { get; set; }
        public string Version { get; set; }

    }
}
