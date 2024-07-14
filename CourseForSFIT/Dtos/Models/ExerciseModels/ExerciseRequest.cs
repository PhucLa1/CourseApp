using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.ExerciseModels
{
    public class ExerciseRequest
    {
        public List<int>? DifficultLevel { get; set; }
        public List<int>? TagId { get; set; }
        public int? Status { get; set; }
        public string? Name { get; set; }
    }
}
