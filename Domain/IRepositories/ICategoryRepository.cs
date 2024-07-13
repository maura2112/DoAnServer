using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Task<int> GetIdCatetegoryOther();
        public Task<Pagination<Category>> GetByStatus(bool? isDeleted, int pageIndex, int pageSize);
        public Task<List<Category>> GetAllNotDeleted();
        public Task<int> GetTotalProjectByCategoryId(int categoryId);

    }
}
