using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<int> GetTotalBids(int projectId);
        Task<int> GetAverageBudget(int projectId);
        Task<Pagination<Project>> ProjectToPagination(int pageIndex, int pageSize);
        public Task<Pagination<Project>> ProjectGetAsync(
            Expression<Func<Project, bool>> filter,
            int pageIndex,
            int pageSize);
        public Task<Pagination<Project>> RecruiterGetAsync(
            Expression<Func<Project, bool>> filter,
            int pageIndex,
            int pageSize);

    }
}
