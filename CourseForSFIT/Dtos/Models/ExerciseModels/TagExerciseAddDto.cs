using FluentValidation;

namespace Dtos.Models.ExerciseModels
{
    public class TagExerciseAddDto : TagExerciseUpdateDto
    {
    }
    public class TagExerciseAddDtoValidator : AbstractValidator<TagExerciseAddDto>
    {
        public TagExerciseAddDtoValidator()
        {
            RuleFor(x => x.Name.Trim()).NotEmpty().WithMessage("Không được để trống nhãn dán");
        }
    }
}
