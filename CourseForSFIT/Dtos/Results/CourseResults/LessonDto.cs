using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results.CourseResults
{
    public class LessonDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ChunkIndex { get; set; }
        public int TotalChunk { get; set; }
        public StatusUpload StatusUpload { get; set; }
    }
}
