using Data.Entities.Base;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class TagBlog : BaseEntities
    {
        [Column("name")]
        [Unique]
        public required string Name { get; set; }
        public ICollection<BlogHasTag>? BlogHasTags { get; set; }
    }
}
