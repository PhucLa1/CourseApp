using FluentValidation;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Models.ExerciseModels
{
    public class ExerciseAddDto
    {
        public required string Name { get; set; }
        public int DifficultLevel { get; set; }
        public required string Content { get; set; }
        public ICollection<int>? TagIds { get; set; }
        public ICollection<TestCaseExerciseAddDto>? TestCaseAddDtos { get; set; }
    }
    public class ExerciseAddDtoValidator : AbstractValidator<ExerciseAddDto>
    {
        public ExerciseAddDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Không được để trống tên bài tập");
            RuleFor(x => x.DifficultLevel)
                .NotEmpty()
                .WithMessage("Không được để trống độ khó")
                .InclusiveBetween(1, 3)
                .WithMessage("Độ khó phải nằm trong khoảng từ 1 đến 3");
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Không được để trống nội dung")
                .Must(BeValidJson)
                .WithMessage("Nội dung phải là một chuỗi JSON hợp lệ")
                .Must(HaveNonEmptyDescription)
                .WithMessage("Trường 'description' trong JSON không được để trống")
                .Must(HaveMeaningfulContent)
                .WithMessage("Mô tả không được chỉ chứa HTML trống");
            RuleFor(x => x.TestCaseAddDtos)
                .NotEmpty()
                .WithMessage("Không được để trống test case");
            RuleForEach(x => x.TestCaseAddDtos)
                .SetValidator(new TestCaseExerciseAddDtoValidator());
        }
        private bool BeValidJson(string content)
        {
            try
            {
                JToken.Parse(content);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool HaveNonEmptyDescription(string content)
        {
            try
            {
                var json = JToken.Parse(content);
                var description = json["description"]?.ToString();
                return !string.IsNullOrEmpty(description);
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool HaveMeaningfulContent(string content)
        {
            var json = JToken.Parse(content);
            var htmlContent = json["description"]?.ToString();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            var textContent = doc.DocumentNode.InnerText.Trim();
            return !string.IsNullOrEmpty(textContent);
        }
    }
}
