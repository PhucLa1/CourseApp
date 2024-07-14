using Dtos.Models.TestCaseModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.ExerciseModels
{
    public class ExerciseAddDto
    {
        public required string ExerciseName { get; set; }
        [Range(1, 3, ErrorMessage = "Độ khó chỉ có giá trị từ 1 đến 3")]
        public int DifficultLevel { get; set; }
        public required string ContentExercise { get; set; }
        public ICollection<int>? TagIds { get; set; }
        public ICollection<TestCaseExerciseAddDto>? TestCaseAddDtos { get; set; }
    }
}
