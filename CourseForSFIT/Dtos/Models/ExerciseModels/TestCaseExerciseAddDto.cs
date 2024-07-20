using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.ExerciseModels
{
    public class TestCaseExerciseAddDto
    {
        public required IFormFile InputData { get; set; }
        public required IFormFile ExpectedOutput { get; set; }
        public bool IsLock { get; set; }
    }
}
