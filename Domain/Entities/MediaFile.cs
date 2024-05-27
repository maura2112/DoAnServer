using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MediaFile
    {
        public long Id { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public int FolderId { get; set; }
        public long Size { get; set; }

        public int UserId { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public AppUser User { get; set; }

        public virtual MediaFolder? MediaFolder { get; set; } = null!;
    }
}
