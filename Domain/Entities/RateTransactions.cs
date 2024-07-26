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
        [ForeignKey("UserProject")]
        public int? ProjectUserId { get; set; }

        public int? ProjectId { get; set; }

        public DateTime? ProjectAcceptedDate { get; set; }

        [ForeignKey("UserBid")]
        public int? BidUserId { get; set; }

        public DateTime? BidCompletedDate { get; set; }

        public bool? Rated { get; set; }
        public bool? RatedOther { get; set; }

        public virtual AppUser? UserProject { get; set; }
        public virtual AppUser? UserBid { get; set; }

    }
}
