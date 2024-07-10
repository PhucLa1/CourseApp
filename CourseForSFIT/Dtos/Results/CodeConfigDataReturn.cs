using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results
{
    public class Run
    {
        public string? Stdout { get; set; }
        public string? Stderr { get; set; }
        public string? Output { get; set; }
        public int Code { get; set; }
        public object? Signal { get; set; }
    }

    public class CodeConfigDataReturn
    {
        public string? Language { get; set; }
        public string? Version { get; set; }
        public Run? Run { get; set; }
    }

}
