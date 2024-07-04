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
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Rating>> Ratings(int uid)
        {
            var ratings = await  _context.Ratings.Where(x=>x.RateToUserId == uid).ToListAsync();
            return ratings;
        }
    }
}
