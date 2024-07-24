using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.CourseModels
{
    public class CourseAddDto
    {
        public required string CourseName { get; set; }       
        public required string Description { get; set; }
        public List<string>? ListLearnAbout { get; set; }
        public List<string>? ListPrepared { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public int Status { get; set; } //1: Draft, 2: Public
        public int CourseTypeId { get; set; }
    }
}
