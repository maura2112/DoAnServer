using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserProject
    {
        public long Id { get; set; }
        public int ProjectId { get; set; }

        public int UserId { get; set; }

        public virtual Project? Project { get; set; } = null!;
        public virtual AppUser? AppUser { get; set; } = null!;
    }
}
