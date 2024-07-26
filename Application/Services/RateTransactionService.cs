using Application.Common;
using Application.DTOs;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RateTransactionService : IRateTransactionService
    {
        private readonly IRateTransactionRepository _repositoty;
        private readonly IMapper _mapper;
        public RateTransactionService(IRateTransactionRepository repositoty, IMapper mapper)
        {
            _repositoty = repositoty;
            _mapper = mapper;
        }
        public async Task<RateTransaction> GetRateTransactionByUsers(int userId1, int userId2)
        {
            var filter = PredicateBuilder.True<Domain.Entities.RateTransaction> ();
            filter = filter.And(item => item.ProjectUserId == userId1);
            filter = filter.And(item => item.ProjectAcceptedDate != null);
            filter = filter.And(item => item.BidCompletedDate != null);
            filter = filter.And(item => item.BidUserId == userId2);
            filter = filter.And(item => item.Rated == false || item.Rated == null);
            var RateTransaction = await _repositoty.GetByFilter(filter);
            if (RateTransaction != null) { 
                return RateTransaction;
            }
            var filter2 = PredicateBuilder.True<Domain.Entities.RateTransaction>();
            filter2 = filter2.And(item => item.ProjectUserId == userId2);
            filter2 = filter2.And(item => item.BidUserId == userId1);
            filter2 = filter2.And(item => item.ProjectAcceptedDate != null);
            filter2 = filter2.And(item => item.BidCompletedDate != null);
            filter2 = filter2.And(item => item.Rated == false || item.Rated == null);
            RateTransaction = await _repositoty.GetByFilter(filter2);
            return RateTransaction;
        }

    }
}
