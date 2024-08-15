using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface  IRateTransactionService
    {
        public Task<RateTransaction> GetRateTransactionByUsers(int userId1, int userId2, int? projectId);
    }
}
