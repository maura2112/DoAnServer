using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Bookmark : BaseEntity
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }

        public DateTime SavedDate { get; set; }

        public virtual AppUser? AppUser { get; set; } = null!;
        public virtual Project? Project { get; set; } = null!;
    }
}
