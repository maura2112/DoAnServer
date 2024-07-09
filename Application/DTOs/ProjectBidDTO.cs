using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProjectBidDTO
    {
        public int ProjectId { get; set; }
        public long BidId { get; set; }
        public string ProjectName { get; set; }

        public string Status { get; set; }
        public int BidBudget { get; set; }

        public int StatusId { get; set; }

        public DateTime TimeBid { get; set; }
        public int Duration { get; set; }

        public DateTime? Deadline { get; set; }

        public string ProjectOwner { get; set; }

        public int ProjectOwnerId { get; set; }

        public bool CanMakeDone { get; set; }
    }
}
