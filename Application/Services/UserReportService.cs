using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserReportService : IUserReportService
    {
        private readonly IReportRepository _repository;
        private readonly IReportCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly PaginationService<ReportDTO> _paginationService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProjectService _projectService;
        private readonly IBidService _bidService;
        private readonly ApplicationDbContext _context;


        public UserReportService(IReportRepository repository, IMapper mapper, PaginationService<ReportDTO> paginationService, IReportCategoryRepository categoryRepository, UserManager<AppUser> userManager, IProjectService projectService, IBidService bidService, ApplicationDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _paginationService = paginationService;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _projectService = projectService;
            _bidService = bidService;
            _context = context;
        }

        public async Task CreateReport(ReportCreateDTO dto)
        {
            var userReport = _mapper.Map<UserReport>(dto);
            userReport.CreatedDate = DateTime.Now;
            await _repository.AddAsync(userReport);
        }

        public async Task<Pagination<ReportDTO>> GetReports(ReportSearchDTO searchDTO)
        {

            var reports =await _repository.GetAll();
            var reportDTOs = reports.Select(  report =>
            {
                var reportDTO = _mapper.Map<ReportDTO>(report);
                var cateReport =  _categoryRepository.GetByIdAsync(reportDTO.ReportCategoryId);
                reportDTO.ReportType = cateReport.Result.Description;
                reportDTO.ReportName = cateReport.Result.Name;
                var userReport = _userManager.FindByIdAsync(report.CreatedBy.ToString());
                reportDTO.NameCreatedBy = userReport.Result.Name;

                if(report.BidId != null) {
                    var Bid = _bidService.GetBidById((long)report.BidId);
                    var userBid = _userManager.FindByIdAsync(Bid.Result.UserId.ToString());
                    reportDTO.BidUser = userBid.Result.Name;
                }else if(report.ProjectId != null) {
                    var project = _projectService.GetDetailProjectForId((int)report.ProjectId);
                    reportDTO.ProjectName = project.Result.Title;
                    var userProject = _userManager.FindByIdAsync(project.Result.CreatedBy.ToString());
                    reportDTO.ProjectUser = userProject.Result.Name;
                }else if (report.UserReportedId != null)
                {
                    var user =  _userManager.FindByIdAsync(report.UserReportedId.ToString());
                    reportDTO.UserReportedName =  user.Result.Name; 
                    reportDTO.UserReportedId = user.Result.Id;
                }
                return reportDTO;
            }).ToList();

            if (searchDTO.typeDes != null)
            {
                reportDTOs = reportDTOs.Where(x=>x.ReportType == searchDTO.typeDes).ToList();
            }
            if(searchDTO.approved != null)
            {
                reportDTOs = reportDTOs.Where(x => x.IsApproved == searchDTO.approved).ToList();
            }
            if (searchDTO.rejected != null)
            {
                reportDTOs = reportDTOs.Where(x => x.IsRejected == searchDTO.rejected).ToList();
            }
            return await _paginationService.ToPagination(reportDTOs, searchDTO.PageIndex, searchDTO.PageSize);
        }

        public async Task<bool> ApproveReport(int id)
        {
            var report = await _repository.GetByIdAsync(id);
            if(report == null)
            {
                return false;
            }else
            {
                report.IsApproved = true;
                _repository.Update(report);
                return true;
            }
        }

        public async Task<bool> RejectReport(int id)
        {
            var report = await _repository.GetByIdAsync(id);
            if (report == null)
            {
                return false;
            }
            else
            {
                report.IsRejected = true;
                _repository.Update(report);
                return true;
            }
        }
    }
}
