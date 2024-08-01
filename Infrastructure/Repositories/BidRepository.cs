using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

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

            var query = from b in _context.Bids
                        join u in _context.Users on b.UserId equals u.Id
                        where b.ProjectId == projectId
                        select u;
            var user = await query.FirstOrDefaultAsync(x => x.AmountBid <= 0 || x.Id == userId);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public async Task<Pagination<Bid>> GetAsyncBid(Expression<Func<Bid, bool>> filter, int pageIndex, int pageSize)
        {
            var items = await _dbSet.Where(filter)
                .AsNoTracking()
                .OrderByDescending(x=>x.UpdatedDate)
                .ToListAsync();
            var totalItem = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var result = new Pagination<Bid>()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalItemsCount = items.Count(),
                Items = totalItem,
            };

            return result;
        }
    }
}
