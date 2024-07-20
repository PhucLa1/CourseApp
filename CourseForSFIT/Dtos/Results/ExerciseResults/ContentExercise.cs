using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results.ExerciseResults
{
    public class ContentExercise
    {
        public TopicExercise? topicExercise { get; set; }
        public int Difficult { get; set; }
        public string? ExerciseName { get; set; }
    }
}
