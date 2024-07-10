using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models
{
    public class UserResultAdd
    {
        public int TestCaseId { get; set; }
        public int UserExerciseId { get; set; }
        public bool IsPass { get; set; }
    }
}
