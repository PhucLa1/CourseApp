using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results.ExerciseResults
{
    public class TopicExercise
    {
        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("constraints")]
        public string? Constraints { get; set; }

        [JsonProperty("input_format")]
        public string? InputFormat { get; set; }

        [JsonProperty("output_format")]
        public string? OutputFormat { get; set; }

        [JsonProperty("input")]
        public List<string>? Input { get; set; }

        [JsonProperty("output")]
        public List<string>? Output { get; set; }

        [JsonProperty("explaintation")]
        public string? Explaintation { get; set; }
    }
}
