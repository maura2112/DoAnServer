using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Statistic
{
    public class StatisticProjects
    {
        public string CategoryName { get; set; }
        public int TotalProjects { get; set; }
        public int MinimumBudget { get; set; }
        public int MaximumBudget { get; set; }
        public float AverageBudget { get; set; }
        public int MinimumDuration { get; set; }
        public int MaximumDuration { get; set; }
        public float AverageDuration { get; set; }
        public int MinimumBid { get; set; }
        public int MaximumBid { get; set; }
        public int AverageBid{ get; set; }
    }
}
