using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models
{
    public class TestCaseSolve
    {
        public int ExerciseId { get; set; }
        public required string ContentCode { get; set; }
        public required string Language { get; set; }
        public required string Version { get; set; }
    }
}
