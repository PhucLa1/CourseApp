using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dtos.Models
{
    public class FileContent
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("content")]
        public string? Content { get; set; }
    }

    public class CodeConfiguration
    {
        [JsonProperty("language")]
        public string? Language { get; set; }

        [JsonProperty("version")]
        public string? Version { get; set; }

        [JsonProperty("files")]
        public List<FileContent>? Files { get; set; }

        [JsonProperty("stdin")]
        public string? Stdin { get; set; }

        [JsonProperty("args")]
        public List<string>? Args { get; set; }

        [JsonProperty("compile_timeout")]
        public int CompileTimeout { get; set; } = 10000;

        [JsonProperty("run_timeout")]
        public int RunTimeout { get; set; } = 3000;

        [JsonProperty("compile_memory_limit")]
        public int CompileMemoryLimit { get; set; } = -1;

        [JsonProperty("run_memory_limit")]
        public int RunMemoryLimit { get; set; } = -1;
    }
}
