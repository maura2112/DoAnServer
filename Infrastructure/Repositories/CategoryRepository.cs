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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllHomePage()
        {
            var items = await _dbSet
                .Where(x => x.IsDeleted == false)
                         .AsNoTracking()
                         .ToListAsync();
            return items;
        }

        public async Task<List<Category>> GetByStatus(bool? isDeleted)
        {
            if (isDeleted != null)
            {
                var items = await _dbSet
                               .Where(x => x.IsDeleted == isDeleted)
                                        .AsNoTracking()
                                        .ToListAsync();
                return items;
            }
            else
            {
                var items = await _dbSet
                        .AsNoTracking()
                        .ToListAsync();
                return items;
            }

        }

        public async Task<int> GetIdCatetegoryOther()
        {
            var categoryOther = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName.Equals("Other"));
            return categoryOther == null ? 0 : categoryOther.Id;
        }
    }
}
