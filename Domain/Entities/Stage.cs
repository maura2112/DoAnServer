using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Stage : BaseEntity
    {
        public int BidStageId { get; set; }
        public int NumberStage { get; set; }
        public string Decription { get; set; }
        public  DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }

        public virtual BidStage? BidStage { get; set; } = null!;
    }
}
