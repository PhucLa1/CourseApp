using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.ExerciseModels
{
    public class CommentExerciseAddDto
    {
        [MinLength(1, ErrorMessage = "Phải nhập nội dung vào")]
        public required string Content { get; set; }
        public int ExerciseId { get; set; }
    }
}
