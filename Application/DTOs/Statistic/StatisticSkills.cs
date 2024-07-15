using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Statistic
{
    public class StatisticSkills
    {
        public string SkillName { get; set; }
        public string CategoryName { get; set; }
        public int TotalApprovedProject { get; set; }
        public int TotalUsers { get; set; }

    }
}
