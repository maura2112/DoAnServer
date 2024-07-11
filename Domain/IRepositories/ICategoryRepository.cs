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
        public Task<List<Category>> GetAllHomePage();
        public Task<List<Category>> GetByStatus(bool? isDeleted);
        
    }
}
