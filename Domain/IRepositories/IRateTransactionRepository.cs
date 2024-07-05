using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IRateTransactionRepository : IGenericRepository<RateTransaction>
    {
        Task<RateTransaction> GetByFilter(Expression<Func<RateTransaction, bool>> filter);
    }
}
