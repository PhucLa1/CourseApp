using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results.TestCaseResults
{
    public class TestCaseReturnDto
    {
        public List<TestCaseDto>? testCaseDtos { get; set; }
        public int totalTestCaseCount { get; set; }
    }
}
