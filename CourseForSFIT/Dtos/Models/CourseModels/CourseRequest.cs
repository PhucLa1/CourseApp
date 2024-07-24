using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.CourseModels
{
    public class CourseRequest
    {
        public List<int>? CourseTypeId { get; set; }
        public string? CourseName { get; set; }
    }
}
