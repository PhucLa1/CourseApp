using FluentValidation;

namespace Dtos.Models.TestCaseModels
{
    public class TestCaseSolve
    {
        public int ExerciseId { get; set; }
        public required string ContentCode { get; set; }
        public required string Language { get; set; }
        public required string Version { get; set; }
        public string? Avatar { get; set; }
    }
    public class TestCaseSolveValidator : AbstractValidator<TestCaseSolve>
    {
        public TestCaseSolveValidator()
        {
            RuleFor(x => x.ExerciseId)
                .GreaterThan(0)
                .WithMessage("ExerciseId phải lớn hơn 0");
            RuleFor(x => x.ContentCode)
                .NotEmpty()
                .WithMessage("ContentCode không được để trống");
            RuleFor(x => x.Language)
                .NotEmpty()
                .WithMessage("Language không được để trống");
            RuleFor(x => x.Version)
                .NotEmpty()
                .WithMessage("Version không được để trống");
            RuleFor(x => x.Avatar)
                .Must(BeValidUrl)
                .When(x => !string.IsNullOrEmpty(x.Avatar))
                .WithMessage("Avatar phải là một URL hợp lệ");
        }
        private bool BeValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
