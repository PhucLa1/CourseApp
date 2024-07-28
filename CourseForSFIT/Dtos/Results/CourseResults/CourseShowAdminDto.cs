using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results.CourseResults
{
    public class CourseShowAdminDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<CourseAdminDto>? courseAdminDtos { get; set; }
    }
}
