using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Pagination<Category>> GetByStatus(bool? isDeleted, int pageIndex, int pageSize)
        {
            if (isDeleted != null)
            {
                var items = await _dbSet.Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Where(x => x.IsDeleted == isDeleted)
                    .AsNoTracking()
                    .ToListAsync();
                var result = new Pagination<Category>()
                {
                    TotalItemsCount = await _dbSet.CountAsync(x => x.IsDeleted == isDeleted),
                    Items = items,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                return result;
 
            }
            else
            {
                
                var items = await _dbSet.Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
                var result = new Pagination<Category>()
                {
                    TotalItemsCount = await _dbSet.CountAsync(),
                    Items = items,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                return result;
            }

        }

        public async Task<List<Category>> GetAllNotDeleted()
        {
            var items = await _dbSet
                .Where(x=>x.IsDeleted == false)
                .AsNoTracking()
                .ToListAsync();
            return items;
        }

        public async Task<int> GetTotalProjectByCategoryId(int categoryId)
        {
            var selectedCategory = await _dbSet.FirstOrDefaultAsync(x => x.Id == categoryId);
            if (selectedCategory == null)
            {
                return 0;
            }
            var totalProject = await _context.Projects
                .Where(p => p.CategoryId == categoryId)
                .CountAsync();

            return totalProject;
        }

        public async Task<int> GetIdCatetegoryOther()
        {
            var categoryOther = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName.Equals("Other"));
            return categoryOther == null ? 0 : categoryOther.Id;
        }
    }
}
