using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RateTransaction :BaseEntity
    {
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }

        public DateTime? AcceptedDate { get; set; }
        [ForeignKey("Bid")]
        public long? BidId { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? TransactionCompletedDate { get; set; }
        public virtual Bid? Bid { get; set; }
        public virtual Project? Project { get; set; }

    }
}
