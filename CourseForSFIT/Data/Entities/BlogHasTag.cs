using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class BlogHasTag : BaseEntities
    {
        [Column("tag_blog_id")]
        public int TagBlogId { get; set; }
        [Column("blog_id")]
        public int BlogId { get; set; }
        public TagBlog? TagBlog { get; set; }
        public Blog? Blog { get; set; }
    }
}
