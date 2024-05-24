using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BidStage : BaseEntity
    {
        public int TotalOfStage { get; set; }

        public bool IsAccepted { get; set; }

        [ForeignKey("Bid")]
        public long BidId { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
        public virtual Bid? Bid { get; set; } = null!;
    }
}
