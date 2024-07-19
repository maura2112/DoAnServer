using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.IRepositories
{
    public interface IBidRepository : IGenericRepository<Bid>
    {
        Task<bool> CheckBidding(int userId, int projectId);

        public Task<Pagination<Bid>> GetAsyncBid(
            Expression<Func<Bid, bool>> filter,
            int pageIndex,
            int pageSize);
    }
}
