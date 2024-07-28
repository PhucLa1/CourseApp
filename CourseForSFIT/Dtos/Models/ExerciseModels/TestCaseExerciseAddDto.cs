using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.ExerciseModels
{
    public class TestCaseExerciseAddDto
    {
        public required IFormFile InputData { get; set; }
        public required IFormFile ExpectedOutput { get; set; }
        public bool IsLock { get; set; }
    }

    public class TestCaseExerciseAddDtoValidator : AbstractValidator<TestCaseExerciseAddDto>
    {
        public TestCaseExerciseAddDtoValidator()
        {
            RuleFor(x => x.InputData)
                .NotEmpty()
                .WithMessage("Input không được để trống");
            RuleFor(x => x.ExpectedOutput)
                .NotEmpty()
                .WithMessage("Output không được để trống");
            RuleFor(x => x.IsLock)
                .NotEmpty()
                .WithMessage("Không được để trống loại test case");
        }
    }
}
