using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Statistic
{
    public class UsersPieChart
    {
        public List<UsersPieChartData> Data { get; set; }
        public int TotalUser { get; set; }
        public int FreelacerCount { get; set; }
        public int RecruiterCount { get; set; }
    }
    public class UsersPieChartData
    {
        public string id { get; set; }
        public string label { get; set; }
        public int value { get; set; }
    }
}
