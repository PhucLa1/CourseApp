﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.CourseModels
{
    public class CourseShowAdminDto
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }
        public List<CourseAdminDto>? courseAdminDtos { get; set; }
    }
}
