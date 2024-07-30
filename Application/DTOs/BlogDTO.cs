using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BlogDTO
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDesction { get; set; }

        public int UserId { get; set; }

        public bool IsPublished { get; set; }
        public bool IsHot { get; set; }
        public bool IsHomePage { get; set; }
        public string Author { get; set; }
        public int CategoryId { get; set; }
        public string BlogImage { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateTime { get; set; }

        public List<RelatedBLogDTO>? relateds { get; set; }
    }

    public class RelatedAdd
    {       
        public int BlogId { get; set; }

        public List<int> RelatedBlogId { get; set; }
    }

    public class RelatedBLogDTO
    {
        public int BlogId { get; set; }
        public string BlogName { get; set; }
        public string  DateString { get; set; }
        public DateTime CreateDate { get; set; }
    }


}
