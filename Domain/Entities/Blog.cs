using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Blog : BaseEntity
    {
        public int CreatedBy {get;set;}
        public string Title { get;set;}
        public int Description { get;set;}
        public DateTime CreatedDate { get;set;}
        public DateTime? UpdatedDate { get;set;}
        public virtual AppUser? AppUser { get; set; } = null!;
        public virtual ICollection<RelatedBlog> RelatedBlogs { get; set; }

    }
}
