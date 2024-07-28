using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.ExerciseModels
{
    public class TagExerciseUpdateDto
    {
        public required string Name { get; set; }
    }
    public class TagExerciseUpdateDtoValidator : AbstractValidator<TagExerciseUpdateDto>
    {
        public TagExerciseUpdateDtoValidator() 
        {
            RuleFor(x =>  x.Name.Trim()).NotEmpty().WithMessage("Không được để trống nhãn dán");
        }
    }
}
