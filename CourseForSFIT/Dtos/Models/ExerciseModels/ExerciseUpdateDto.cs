using Dtos.Results.ExerciseResults;
using FluentValidation;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.ExerciseModels
{
    public class ExerciseUpdateDto
    {
        public required TopicExercise topicExercise { get; set; }
        public int Difficult { get; set; }
        public required string Name { get; set; }
        public ICollection<int>? TagIds { get; set; }
    }
    public class ExerciseUpdateDtoValidator : AbstractValidator<ExerciseUpdateDto>
    {
        public ExerciseUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("Không được để trống tên bài tập");
            RuleFor(x => x.Difficult)
                .NotEmpty()
                .WithMessage("Không được để trống độ khó")
                .InclusiveBetween(1, 3)
                .WithMessage("Độ khó phải nằm trong khoảng từ 1 đến 3");
            RuleFor(x => x.topicExercise.Description)
                .NotEmpty()
                .WithMessage("Không được để trống phần mô tả")
                .Must(HaveMeaningfulContent)
                .WithMessage("Description không được chỉ chứa HTML trống");
        }
        private bool HaveMeaningfulContent(string? htmlContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            var textContent = doc.DocumentNode.InnerText.Trim();
            return !string.IsNullOrEmpty(textContent);
        }
    }
}
