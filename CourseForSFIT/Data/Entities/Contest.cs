using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Contest : BaseEntities
    {
        [Column("contest_name")]
        public required string ContestName { get; set; }
        [Column("description")]
        public required string Description { get; set; }
        public ICollection<ContestExercise>? ContestExercises { get; set; }
        public ICollection<UserJoin>? UserJoins { get; set; }
    }
}
