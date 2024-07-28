using FluentValidation;

namespace Dtos.Models.CourseTypeModels
{
    public class CourseTypeUpdate : CourseTypeAdd
    {
    }
    public class CourseTypeUpdateValidator : AbstractValidator<CourseTypeUpdate>
    {
        public CourseTypeUpdateValidator()
        {
            RuleFor(x => x.Name.Trim())
                .NotEmpty()
                .WithMessage("Không được để trống tên");
        }
    }
}
