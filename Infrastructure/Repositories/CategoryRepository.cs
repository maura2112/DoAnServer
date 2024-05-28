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
        public async Task<int> GetIdCatetegoryOther()
        {
            var categoryOther = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName.Equals("Other"));
            return categoryOther == null ? 0 : categoryOther.Id;
        }
    }
}
