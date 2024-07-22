using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Models.UserModels
{
    public class UserUpdateDto
    {
        public IFormFile? Avatar { get; set; }
        public string? NickName { get; set; }
        public int? SchoolYear { get; set; }
        public string? Class { get; set; }
    }
}
