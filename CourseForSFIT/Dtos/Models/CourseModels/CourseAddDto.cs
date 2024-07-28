using Data.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.CourseModels
{
    public class CourseAddDto
    {
        public required string Name { get; set; }       
        public required string Description { get; set; }
        public List<string>? ListLearnAbout { get; set; }
        public List<string>? ListPrepared { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public StatusCourse Status { get; set; } //1: Draft, 2: Public
        public int CourseTypeId { get; set; }
    }
    public class CourseAddDtoValidator : AbstractValidator<CourseAddDto>
    {
        public CourseAddDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tên khóa học không được để trống");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Mô tả khóa học không được để trống");
            RuleFor(x => x.ListLearnAbout)
                .Must(list => list == null || list.All(item => !string.IsNullOrEmpty(item.Trim())))
                .WithMessage("Tất cả các mục trong ListLearnAbout phải có giá trị");
            RuleFor(x => x.ListPrepared)
                .Must(list => list == null || list.All(item => !string.IsNullOrEmpty(item.Trim())))
                .WithMessage("Tất cả các mục trong ListPrepared phải có giá trị");
            RuleFor(x => x.Thumbnail)
                .Must(BeAValidImage)
                .When(x => x.Thumbnail != null)
                .WithMessage("Thumbnail phải là một tệp hình ảnh hợp lệ (jpeg, png, bmp)");
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Trạng thái không hợp lệ");
            RuleFor(x => x.CourseTypeId)
                .GreaterThan(0)
                .WithMessage("CourseTypeId phải lớn hơn 0");
        }

        private bool BeAValidImage(IFormFile? file)
        {
            var allowedExtensions = new[] { ".jpeg", ".jpg", ".png", ".bmp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }
    }
}
