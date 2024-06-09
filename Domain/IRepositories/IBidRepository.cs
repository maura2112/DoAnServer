using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IBidRepository : IGenericRepository<Bid>
    {
        Task<bool> CheckBidding(int userId, int projectId);
    }
}
