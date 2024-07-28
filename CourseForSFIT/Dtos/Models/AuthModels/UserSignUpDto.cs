using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Models.AuthModels
{
    public class UserSignUpDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Mật khẩu phải dài ít nhất 8 ký tự và chứa ít nhất một chữ in hoa và một chữ số.")]
        public required string Password { get; set; }
        public required string RePassword { get; set; }
    }
    public class UserSignUpDtoValidator : AbstractValidator<UserSignUpDto>
    {
        public UserSignUpDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Họ không được để trống");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Tên không được để trống");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email không được để trống.")
                .EmailAddress()
                .WithMessage("Phải đúng định dạng email");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Mật khẩu không được để trống.")
                .Matches(@"^(?=.*[A-Z])(?=.*\d).{8,}$")
                .WithMessage("Mật khẩu phải dài ít nhất 8 ký tự và chứa ít nhất một chữ in hoa và một chữ số.");
            RuleFor(x => x.RePassword)
                .NotEmpty()
                .WithMessage("Xác nhận lại mật khẩu không được để trống.")
                .Equal(x => x.Password)
                .WithMessage("Xác nhận mật khẩu và mật khẩu không trùng khớp nhau.");
        }
    }
}
