using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.ExerciseModels
{
    public class ExerciseHasTagAddDto
    {
        public int TagExerciseId { get; set; }
        public int ExerciseId { get; set; }
    }
}
