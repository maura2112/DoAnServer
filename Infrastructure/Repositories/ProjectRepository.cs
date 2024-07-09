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
            var itemCount = await _context.Projects.CountAsync();
            var items = await _context.Projects.Skip((pageIndex - 1) * pageSize)
                .AsNoTracking()
                .Where(x=>x.IsDeleted==false && x.StatusId == 2)
                .ToListAsync();
            var result = new Pagination<Project>()
            {
                TotalItemsCount = itemCount,
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return result;
        }

        public async Task<Pagination<Project>> ProjectGetAsync(Expression<Func<Project, bool>> filter, int pageIndex, int pageSize)
        {
            var items = await _dbSet.Where(filter)
                .AsNoTracking()
                .ToListAsync();
            var totalItem = items.Skip((pageIndex - 1) * pageSize).Where(x => x.IsDeleted == false && x.StatusId == 2).ToList();
            var result = new Pagination<Project>()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalItemsCount = items.Count(),
                Items = totalItem,
            };

            return result;
        }

        public async Task<List<Project>> ProjectGetAll()
        {
            var items = await _dbSet
                .AsNoTracking()
                .Where(x => x.IsDeleted == false && x.StatusId == 2)
                .ToListAsync();
            return items;
        }
    }
}
