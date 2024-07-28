using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.CourseModels
{
    public class LessonUploadChunkFile
    {
        public required IFormFile ChunkFile { get; set; }
        public int ChunkIndex { get; set; }
        public int TotalChunk { get; set; }
    }
}
