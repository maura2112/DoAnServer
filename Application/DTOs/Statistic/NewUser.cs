using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Statistic
{
    public class NewUser
    {
        public int TotalUserCount { get; set; }
        public int FreelancerCount { get; set; }
        public int RecruiterCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FormattedDate { get; set; }
    }
}
