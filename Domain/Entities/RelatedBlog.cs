using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RelatedBlog : BaseEntity
    {
        public int BlogId { get; set; }
        public int RelatedBlogId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Blog? Blog { get; set; } = null!;

    }
}

