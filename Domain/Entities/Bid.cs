using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Bid 
    {
        public long Id { get; set; }
        public int ProjectId { get; set; }
        public int? UserId { get; set; }
        public string Proposal { get; set; }

        public int Duration { get; set; }

        public int Budget { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public int? TotalOfStage { get; set; }
        public virtual ICollection<BidStage>? BidStages { get; set; }

        public virtual Project? Project { get; set; } = null!;
        public virtual AppUser? AppUser { get; set; } = null!;

        
    }
}
