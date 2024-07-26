using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ReportsController : ApiControllerBase
    {
        
        private readonly IReportCategoryService _reportCategoryService ;
        private readonly IUserReportService _userReportService;
        private readonly ICurrentUserService _currentUserService;

        public ReportsController(IReportCategoryService reportCategoryService, IUserReportService userReportService, ICurrentUserService currentUserService)
        {

            _reportCategoryService = reportCategoryService;
            _userReportService = userReportService;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        [Route(Common.Url.Report.Categories)]
        public async Task<IActionResult> ReportCategories(string type)
        {
            var reportCategories = await _reportCategoryService.Categories(type);
            return Ok(reportCategories);
        }

        [HttpPost]
        [Route(Common.Url.Report.Create)]
        public async Task<IActionResult> CreateReport([FromForm] ReportCreateDTO dto)
        {
            var userId = _currentUserService.UserId;
            dto.CreatedBy = userId;
            await _userReportService.CreateReport(dto);
            return Ok(dto);
        }

        [HttpGet]
        [Route(Common.Url.Report.Reports)]
        [RoleAuthorizeAttribute("Admin")]
        public async Task<IActionResult> GetReports([FromQuery] ReportSearchDTO dto)
        {
            var rpDTO = await _userReportService.GetReports(dto);
            return Ok(rpDTO);
        }

        [HttpPost]
        [Route(Common.Url.Report.Approve)]
        [RoleAuthorizeAttribute("Admin")]
        public async Task<IActionResult> Approve([FromForm] int id)
        {
            var rpDTO = await _userReportService.ApproveReport(id);
            if (rpDTO)
            {
                return Ok(new{
                    success = true,
                    message = "Đã duyệt báo cáo này !"
                });
            }
            return BadRequest(new
            {
                success = false,
                message = "Duyệt báo cáo không thành công !"
            });

        }

        [HttpPost]
        [Route(Common.Url.Report.Reject)]
        [RoleAuthorizeAttribute("Admin")]
        public async Task<IActionResult> Reject([FromForm] int id)
        {
            var rpDTO = await _userReportService.RejectReport(id);
            if (rpDTO)
            {
                return Ok(new
                {
                    success = true,
                    message = "Đã từ chối báo cáo này !"
                });
            }
            return BadRequest(new
            {
                success = false,
                message = "Từ chối báo cáo không thành công !"
            });

        }
    }
}
