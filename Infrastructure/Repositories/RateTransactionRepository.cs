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

namespace Infrastructure.Repositories
{
    public class RateTransactionRepository :  GenericRepository<RateTransaction>,IRateTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public RateTransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RateTransaction> GetByFilter(Expression<Func<RateTransaction, bool>> filter)
        {
            var rateTransaction = await _context.RateTransactions.Where(filter)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();
            return rateTransaction;
        }
    }
}
