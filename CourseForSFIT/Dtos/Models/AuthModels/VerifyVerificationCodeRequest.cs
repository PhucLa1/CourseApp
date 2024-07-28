using FluentValidation;

namespace Dtos.Models.AuthModels
{
    public record VerifyVerificationCodeRequest
    {
        public required string Email { get; set; }
        public required string Code { get; set; }
    }
    public class VerifyVerificationCodeRequestValidator : AbstractValidator<VerifyVerificationCodeRequest>
    {
        public VerifyVerificationCodeRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email không được để trống")
                .EmailAddress()
                .WithMessage("Không đúng định dạng email");
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Mã code không được để trống")
                .Length(6)
                .WithMessage("Mã code chỉ được đúng 6 kí tự")
                .Matches(@"^\d+$")
                .WithMessage("Mật khẩu chỉ được bao gồm số.");
        }
    }
}
