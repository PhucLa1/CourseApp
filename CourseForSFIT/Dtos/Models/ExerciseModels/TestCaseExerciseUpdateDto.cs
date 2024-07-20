using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.ExerciseModels
{
    public class TestCaseExerciseUpdateDto 
    {
        public IFormFile? InputData { get; set; }
        public IFormFile? ExpectedOutput { get; set; }
        public bool IsLock { get; set; }
    }
}
