﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results.TestCaseResults
{
    public class TestCaseDto
    {
        public int Id { get; set; }
        public string? InputData { get; set; }
        public int ExerciseId { get; set; }
        public required string ExpectedOutput { get; set; }
        public bool IsLock { get; set; }
    }
}
