using Application.IServices;
using Domain.Entities;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReportCategoryService : IReportCategoryService
    {
        private readonly IReportCategoryRepository _reportCategoryRepository;

        public ReportCategoryService(IReportCategoryRepository reportCategoryRepository)
        {
            _reportCategoryRepository = reportCategoryRepository;
        }



        public async Task<List<ReportCategory>> Categories(string type)
        {
            var reportCates = await _reportCategoryRepository.GetAll();
            reportCates = reportCates.Where(x=>x.Description.Equals(type) || x.Description.Equals("all")).OrderByDescending(x => x.Id).ToList();
            return reportCates;
        }
    }
}
