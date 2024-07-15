using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Statistic
{
    public class StatisticUsers 
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public int TotalCompletedProjects { get; set; }
        public int TotalPositiveRatings { get; set; }
        public int TotalNegativeRatings { get; set; }

    }
}
