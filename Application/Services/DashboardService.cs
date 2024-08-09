using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.DashboardDTO;
using Application.DTOs.Statistic;
using Application.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public DashboardService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<RecruiterDTO> GetRecruiterDashboard()
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return null;
            }
            else
            {
                RecruiterDTO result = new RecruiterDTO()
                {
                    TotalProjects = await _context.Projects.CountAsync(x => x.CreatedBy == userId && x.IsDeleted == false),
                    TotalCompletedProjects = await _context.Projects.CountAsync(x => x.CreatedBy == userId && x.StatusId == 6 && x.IsDeleted == false),
                    TotalDoingProjects = await _context.Projects.CountAsync(x => x.CreatedBy == userId && x.StatusId == 3 && x.IsDeleted == false),
                    TotalPendingProjects = await _context.Projects.CountAsync(x => x.CreatedBy == userId && x.StatusId == 1 && x.IsDeleted == false),
                    TotalPostedProjects = await _context.Projects.CountAsync(x => x.CreatedBy == userId && x.StatusId == 2 && x.IsDeleted == false)
                };

                var projectsPerCate = await _context.Categories
                    .Select(c => new CategoriesPieChart
                    {
                        CategoryName = c.CategoryName,
                        TotalProjects = c.Projects.Count(p => p.CreatedBy == userId && p.StatusId != 1 && p.IsDeleted == false)
                    })
                    .Where(cp => cp.TotalProjects > 0) // Lọc những mục có TotalProjects > 0
                    .ToListAsync();

                result.ProjectsPerCate = projectsPerCate;

                return result;
            }

        }

        public async Task<FreelancerDTO> GetFreelancerDashboard()
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return null;
            }
            else
            {
                FreelancerDTO result = new FreelancerDTO()
                {
                    TotalProjects = await _context.Projects.CountAsync(x => x.CreatedBy == userId && x.StatusId != 1 && x.IsDeleted == false),
                    TotalCompletedProjects = await _context.Projects.CountAsync(x => x.CreatedBy == userId && x.StatusId == 6 && x.IsDeleted == false),
                    TotalDoingProjects = await _context.Projects.CountAsync(x => x.CreatedBy == userId && x.StatusId == 3 && x.IsDeleted == false),
                    TotalPendingProjects = await _context.Projects.CountAsync(x => x.CreatedBy == userId && x.StatusId == 1 && x.IsDeleted == false),
                    TotalPostedProjects = await _context.Projects.CountAsync(x => x.CreatedBy == userId && x.StatusId == 2 && x.IsDeleted == false),
                    TotalBids = await _context.Bids.CountAsync(x=>x.UserId == userId)
                };
                var projectsPerCate = await _context.Categories
                    .Select(c => new CategoriesPieChart
                    {
                        CategoryName = c.CategoryName,
                        TotalProjects = c.Projects.Count(p => p.CreatedBy == userId && p.StatusId != 1 && p.IsDeleted == false)
                    })
                    .ToListAsync();
                result.ProjectsPerCate = projectsPerCate;

                return result;
            }
        }
    }
}
