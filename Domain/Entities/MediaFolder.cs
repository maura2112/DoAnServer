using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MediaFolder : BaseEntity
    {
        public MediaFolder()
        {
            MediaFiles = new List<MediaFile>();
        }
        public string Name { get; set; }
        public virtual ICollection<MediaFile> MediaFiles { get; set; }
    }
}
