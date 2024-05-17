
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task AddAsync(T entity);
        public Task AddRangeAsync(IEnumerable<T> entities);
        public Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        public Task<bool> AnyAsync();
        public Task<int> CountAsync(Expression<Func<T, bool>> filter);
        public Task<int> CountAsync();
        public Task<T> GetByIdAsync(object id);
        public Task<Pagination<T>> GetAsync(
           Expression<Func<T, bool>> filter,
           int pageIndex,
           int pageSize);
        public Task<Pagination<T>> ToPagination(int pageIndex, int pageSize);

        public Task<List<T>> GetAll();

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
        public void Update(T entity);
        public void UpdateRange(IEnumerable<T> entities);
        public void Delete(T entity);
        public void DeleteRange(IEnumerable<T> entities);
        public Task Delete(object id);
    }
}
