using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetAverageBudget(int projectId)
        {
            int totalBidding = 0;
            int totalBudget = 0;
            float avgBudget = 0;
            var listBidding = await _context.Bids.Where(x => x.ProjectId == projectId).ToListAsync();
            totalBidding = listBidding.Count();
            foreach (var bidding in listBidding)
            {
                totalBudget += bidding.Budget;
            }
            if (totalBidding > 0)
            {
                avgBudget = (float)totalBudget / (float)totalBidding;
                return (int)Math.Round(avgBudget);
            }
            else
            {
                return 0;
            }
        }
        public async Task<int> GetTotalBids(int projectId)
        {
            return await _context.Bids.Where(x => x.ProjectId == projectId).CountAsync();
        }

        public async Task<Pagination<Project>> ProjectToPagination(int pageIndex, int pageSize)
        {
            
            var items = await _context.Projects
                .Where(x => x.IsDeleted == false && x.StatusId == 2)
                .OrderBy(x=>x.UpdatedDate)
                .AsNoTracking()
                .ToListAsync();
            var totalItem = items.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToList();
            var result = new Pagination<Project>()
            {
                TotalItemsCount = await _dbSet.CountAsync(x => x.IsDeleted == false && x.StatusId == 2),
                Items = totalItem,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return result;
        }

        public async Task<Pagination<Project>> ProjectGetAsync(Expression<Func<Project, bool>> filter, int pageIndex, int pageSize)
        {
            var items = await _dbSet.Where(filter)
                .Where(x => x.IsDeleted == false && x.StatusId == 2).OrderBy(x=>x.UpdatedDate)
                .AsNoTracking()
                .ToListAsync();
            var totalItem = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var result = new Pagination<Project>()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalItemsCount = items.Count(),
                Items = totalItem,
            };

            return result;
        }

        public async Task<Pagination<Project>> RecruiterGetAsync(Expression<Func<Project, bool>> filter, int pageIndex, int pageSize)
        {
            var items = await _dbSet.Where(filter)
                .Where(x => x.IsDeleted == false )
                .OrderBy(x=>x.UpdatedDate)
                .AsNoTracking()
                .ToListAsync();
            var totalItem = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var result = new Pagination<Project>()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalItemsCount = items.Count(x => x.IsDeleted == false),
                Items = totalItem,
            };

            return result;
        }
    }
}
