using FluentValidation;

namespace Dtos.Models.CourseModels
{
    public class ChapterAdd
    {
        public int CourseId { get; set; }
        public required string Name { get; set; }
    }
    public class ChapterAddValidator : AbstractValidator<ChapterAdd>
    {
        public ChapterAddValidator()
        {
            RuleFor(chapter => chapter.Name.Trim())
                .NotEmpty().WithMessage("Tên không được để trống.");
        }
    }
}
