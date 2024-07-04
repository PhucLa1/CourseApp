using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class User : BaseEntities
    {
        [Column("first_name")]
        public required string FirstName { get; set; }
        [Column("last_name")]
        public required string LastName { get; set; }
        [Column("avatar")]
        public string? Avatar { get; set; }
        [Column("email")]
        public required string Email { get; set; }
        [Column("password")]
        public required string  Password { get; set; }
        [Column("nick_name")]
        public string? NickName { get; set; }
        [Column("school_year")]
        public int? SchoolYear { get; set; }
        [Column("class")]
        public string? Class { get; set; }
        [Column("role")]
        public int Role { get; set; }
        [Column("join_date")]
        public DateTime JoinDate { get; set; }
        [Column("code")]
        public string? Code { get; set; }
        [Column("expired_time")]
        public DateTime? ExpiredTime { get; set; }
        [Column("achivements")]
        public string? Achivements { get; set; }
    }
}
