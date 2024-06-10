using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
