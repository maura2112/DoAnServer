using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BlogDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public string Author { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateTime { get; set; }
    }
}
