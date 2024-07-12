using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BlogSearch : SearchDTO
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

        public string? AuthorName { get; set; }
    }
}
