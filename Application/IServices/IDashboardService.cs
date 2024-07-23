using Application.DTOs.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.DashboardDTO;

namespace Application.IServices
{
    public interface IDashboardService
    {
        public Task<RecruiterDTO> GetRecruiterDashboard();
        public Task<FreelancerDTO> GetFreelancerDashboard();
    }
}
