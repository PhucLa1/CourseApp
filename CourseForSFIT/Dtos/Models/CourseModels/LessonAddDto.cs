using Data.Entities;
using FluentValidation;

namespace Dtos.Models.CourseModels
{
    public class LessonAddDto
    {
        public required string Name { get; set; }
        public int ChapterId { get; set; }
    }
    public class LessonAddDtoValidator : AbstractValidator<LessonAddDto>
    {
        public LessonAddDtoValidator()
        {
            RuleFor(x => x.Name.Trim())
                .NotEmpty()
                .WithMessage("Không được bỏ trống name");
            RuleFor(x => x.ChapterId)
                .NotEmpty()
                .WithMessage("Không được bỏ trống chapter id");
        }
    }
}
