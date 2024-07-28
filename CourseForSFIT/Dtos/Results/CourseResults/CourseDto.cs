using Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results.CourseResults
{
    public class CourseDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public List<string>? ListLearnAbout { get; set; }
        public List<string>? ListPrepared { get; set; }
        public string? Thumbnail { get; set; }
        public StatusCourse Status { get; set; } //1: Draft, 2: Public
        public int CourseTypeId { get; set; }
    }
}
