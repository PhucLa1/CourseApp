using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class BlogComment : BaseEntities
    {
        [Column("content")]
        public required string Content { get; set; }
        [Column("blog_id")]
        public int BlogId { get; set; }
    }
}
