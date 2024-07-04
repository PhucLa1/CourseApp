using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class UserJoin : BaseEntities
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("contest_id")]
        public int ContestId { get; set; }
    }
}
