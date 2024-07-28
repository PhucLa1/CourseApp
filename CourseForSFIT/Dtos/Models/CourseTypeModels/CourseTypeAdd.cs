using FluentValidation;

namespace Dtos.Models.CourseTypeModels
{
    public class CourseTypeAdd
    {
        public required string Name { get; set; }
    }
    public class CourseTypeAddValidator : AbstractValidator<CourseTypeAdd>
    {
        public CourseTypeAddValidator()
        {
            RuleFor(x => x.Name.Trim())
                .NotEmpty()
                .WithMessage("Không được để trống tên");
        }
    }
}
