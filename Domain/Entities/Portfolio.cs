using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Portfolio : BaseEntity
    {
        public int UserId { get; set; }
        public int MediaFileId { get; set; }
        public virtual AppUser? AppUser { get; set; } = null!;
        public virtual MediaFile? MediaFile { get; set; } = null!;
    }
}
