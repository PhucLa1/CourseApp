using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Models.AuthModels
{
    public record ResetPassword
    {
        public required string Email { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Mật khẩu phải dài ít nhất 8 ký tự và chứa ít nhất một chữ in hoa và một chữ số.")]
        public required string Password { get; set; }
        public required string RePassword { get; set; }
    }

    public class ResetPasswordValidator : AbstractValidator<ResetPassword>
    {
        public ResetPasswordValidator() 
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email phải có định dạng hợp lệ.")
                .NotEmpty()
                .WithMessage("Email không được để trống");
            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(@"^(?=.*[A-Z])(?=.*\d).{8,}$")
                .WithMessage("Mật khẩu phải dài ít nhất 8 ký tự và chứa ít nhất một chữ in hoa và một chữ số.");
            RuleFor(x => x.RePassword)
                .NotEmpty()
                .Equal(x => x.Password)
                .WithMessage("Xác nhận mật khẩu không khớp với mật khẩu.");
        }
    }
}
