using Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Statistic;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly ApplicationDbContext _context;
        public StatisticService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            
        }
        public async Task<List<CategoriesPieChart>> GetCategoryPieChartData()
        {
            var result = await _context.Categories
                .Select(c => new CategoriesPieChart
                {
                    CategoryName = c.CategoryName,
                    TotalProjects = c.Projects.Count()
                })
                .ToListAsync();

            return result;
        }

        public async Task<ProjectsPieChart> GetProjectPieChartData()
        {
            var totalProjects = await _context.Projects.CountAsync(p => p.StatusId == 2);
            var completedProjects = await _context.Projects.CountAsync(p => p.StatusId == 6);

            var result = new ProjectsPieChart
            {
                TotalAppovedProjects = totalProjects,
                CompletedProjects = completedProjects
            };

            return result;
        }

        public async Task<UsersPieChart> GetUserPieChartData()
        {
            var freelancerCount = await _context.UserRoles
                .Where(ur => ur.RoleId == 1)
                .Select(ur => ur.UserId)
                .CountAsync();
            var recruiterCount = await _context.UserRoles
                .Where(ur => ur.RoleId == 2)
                .Select(ur => ur.UserId)
                .CountAsync();

            var result = new UsersPieChart
            {
                FreelacerCount = freelancerCount,
                RecruiterCount = recruiterCount
            };

            return result;
        }

        public async Task<List<NewUser>> GetNewUserData()
        {
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);

            var newUserCounts = await _context.Users
                .Join(_context.UserRoles,
                    u => u.Id,
                    ur => ur.UserId,
                    (u, ur) => new { User = u, UserRole = ur })
                .Where(j => (j.UserRole.RoleId == 1 || j.UserRole.RoleId == 2) && j.User.CreatedDate >= thirtyDaysAgo)
                .GroupBy(j => j.User.CreatedDate.Date)
                .Select(g => new NewUser
                {
                    CreatedDate = g.Key,
                    TotalUserCount = g.Count(),
                    FreelancerCount = g.Count(j => j.UserRole.RoleId == 1),
                    RecruiterCount = g.Count(j => j.UserRole.RoleId == 2)
                })
                .ToListAsync();

            // Tổng số user là tổng của freelancerCount và recruiterCount
            foreach (var userCount in newUserCounts)
            {
                userCount.TotalUserCount = userCount.FreelancerCount + userCount.RecruiterCount;
            }

            return newUserCounts;
        }
        
    }
}
