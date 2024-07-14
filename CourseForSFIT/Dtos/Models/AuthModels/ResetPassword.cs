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
}
