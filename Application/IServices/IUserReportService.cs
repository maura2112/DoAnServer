using Application.DTOs;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IUserReportService
    {
        public Task CreateReport(ReportCreateDTO dto);
        public Task<Pagination<ReportDTO>> GetReports(ReportSearchDTO searchDTO);
        Task<bool> ApproveReport(int id);
    }
}
