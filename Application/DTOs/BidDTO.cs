using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BidDTO 
    {
        public long Id { get; set; }
        public int ProjectId { get; set; }

        public int? UserId { get; set; }

        public string Proposal { get; set; }

        public string Duration { get; set; }

        public int Budget { get; set; }

        public int StatusId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedTime { get; set; }

        public virtual ProjectDTO? Project { get; set; } = null!;
        public virtual AppUserDTO? AppUser { get; set; } = null!;
    }
}
