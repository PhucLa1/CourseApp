using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Blog : BaseEntities
    {
        [Column("title")]
        public required string Title { get; set; }
        [Column("content")]
        public required string Content { get; set; }
        public ICollection<BlogHasTag>? BlogHasTags { get; set; }
        public ICollection<BlogComment>? BlogComments { get; set; }
    }
}
