using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Blog : BaseEntity
    {
        public int CreatedBy {get;set;}
        public string Title { get;set;}
        public string ShortDescription { get;set;}
        public string Description { get;set;}
        [ForeignKey("Category")]
        public int CategoryId { get;set;}
        public string BlogImage { get;set;}

        public bool IsPublished { get; set; }
        public bool IsHot { get; set; }
        public bool IsHomePage { get; set; }
        public Category Category { get;set;}
        public DateTime CreatedDate { get;set;}
        public DateTime? UpdatedDate { get;set;}
        public virtual AppUser? AppUser { get; set; } = null!;
        public virtual ICollection<RelatedBlog> RelatedBlogs { get; set; }

    }
}
