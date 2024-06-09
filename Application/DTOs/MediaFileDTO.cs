using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class MediaFileDTO
    {
        public long Id { get; set; }
        public string FileName { get; set; }

        public string? Description { get; set; }

        public string? Title { get; set; }

        public int? UserId { get; set; } // null able
    }
}
