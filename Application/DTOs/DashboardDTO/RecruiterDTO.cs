using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Statistic;

namespace Application.DTOs.DashboardDTO
{
    public class RecruiterDTO
    {
        public int TotalProjects { get; set; }
        // - TotalPendingProjects
        public int TotalPostedProjects { get; set; }
        public int TotalDoingProjects { get; set;}
        public int TotalCompletedProjects { get; set; }
        public int TotalPendingProjects { get; set; }
        public List<CategoriesPieChart> ProjectsPerCate { get; set; }
        

    }
}
