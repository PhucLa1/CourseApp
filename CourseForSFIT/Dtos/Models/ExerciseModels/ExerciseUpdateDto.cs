using Dtos.Results.ExerciseResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.ExerciseModels
{
    public class ExerciseUpdateDto
    {
        public required TopicExercise topicExercise { get; set; }
        public int Difficult { get; set; }
        public required string ExerciseName { get; set; }
        public ICollection<int>? TagIds { get; set; }
    }
}
