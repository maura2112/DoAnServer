
using Azure.Core;
using Domain.Common;
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
using static Domain.Exceptions.Constant;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
    {
        protected DbSet<T> _dbSet = context.Set<T>();

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await _dbSet.AddRangeAsync(entities);

        #region  Read
        public async Task<bool> AnyAsync()
            => await _dbSet.AnyAsync();

        public async Task<int> CountAsync()
            => await _dbSet.CountAsync();

        public async Task<T> GetByIdAsync(object id)
            => await _dbSet.FindAsync(id)
            ?? throw new ArgumentNullException(ErrorMessage.NotFoundMessage);
        
        public async Task<Pagination<T>> ToPagination(int pageIndex, int pageSize)
        {
            var itemCount = await _dbSet.CountAsync();
            var items = await _dbSet.Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<T>()
            {
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<T>> GetAsync(
            Expression<Func<T, bool>> filter,
            int pageIndex ,
            int pageSize )
        {
            var items = await _dbSet.Where(filter)
                                    .AsNoTracking()
                                    .ToListAsync();
            var totalItem = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var result = new Pagination<T>()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalItemsCount = items.Count(),
                Items = totalItem,
            };

            return result;
        }


     

        #endregion
        #region Update & delete

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void UpdateRange(IEnumerable<T> entities)
            => _dbSet.UpdateRange(entities);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public void DeleteRange(IEnumerable<T> entities)
            => _dbSet.RemoveRange(entities);

        public async Task Delete(object id)
        {
            T entity = await GetByIdAsync(id);
            Delete(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
         => await _dbSet.AnyAsync(filter);

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            return filter == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(filter);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
         => await _dbSet.IgnoreQueryFilters()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(filter)
                            ?? throw new ArgumentNullException(ErrorMessage.NotFoundMessage);

        
        #endregion
    }
}
