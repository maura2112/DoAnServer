using Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class DashboardController : ApiControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Route(Common.Url.Dashboard.RecruiterDashboard)]
        public async Task<IActionResult> GetRecruiterDashboard()
        {
            var result = await _dashboardService.GetRecruiterDashboard();
            if (result == null)
            {
                return BadRequest(new
                {
                    message = "Không thể lấy dữ liệu"
                });
            }
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpGet]
        [Route(Common.Url.Dashboard.FreelancerDashboard)]
        public async Task<IActionResult> GetFreelancerDashboard()
        {
            var result = await _dashboardService.GetFreelancerDashboard();
            if (result == null)
            {
                return BadRequest(new
                {
                    message = "Không thể lấy dữ liệu"
                });
            }
            return Ok(new
            {
                success = true,
                data = result
            });

        }
    }
}
