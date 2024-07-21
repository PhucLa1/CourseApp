using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Results.UserResults
{
    public class UserInfoDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public string? NickName { get; set; }
        public int? SchoolYear { get; set; }
        public string? Class { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<string>? AchivementsDeserialize { get; set; }
    }
}
