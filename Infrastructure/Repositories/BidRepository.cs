using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BidRepository : GenericRepository<Bid>, IBidRepository
    {
        private readonly ApplicationDbContext _context;

        public BidRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> CheckBidding(int userId, int projectId)
        {
            int count = 0;
            var listBidding = await _context.Bids.Where(x => x.ProjectId == projectId).ToListAsync();
            foreach (var bidding in listBidding)
            {
                if (bidding.UserId == userId)
                {
                    count++;
                };
            }
            if(count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
