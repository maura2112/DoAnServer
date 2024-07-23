using Application.DTOs.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DashboardDTO
{
    public class FreelancerDTO
    {
        public int TotalProjects { get; set; }
        // - TotalPendingProjects
        public int TotalPostedProjects { get; set; }
        public int TotalDoingProjects { get; set; }
        public int TotalCompletedProjects { get; set; }
        public int TotalPendingProjects { get; set; }
        public List<CategoriesPieChart> ProjectsPerCate { get; set; }
        public int TotalBids { get; set; }
    }
}
