using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
