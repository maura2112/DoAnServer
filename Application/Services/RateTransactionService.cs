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
        private readonly ICurrentUserService _currentUserService;
        public RateTransactionService(IRateTransactionRepository repositoty, IMapper mapper, ICurrentUserService currentUserService)
        {
            _repositoty = repositoty;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<RateTransaction> GetRateTransactionByUsers(int userId1, int userId2)
        {
            var userCurrenId = _currentUserService.UserId;
            var filter = PredicateBuilder.True<Domain.Entities.RateTransaction> ();
            filter = filter.And(item => item.ProjectUserId == userId1 || item.ProjectUserId == userId2);
            filter = filter.And(item => item.ProjectAcceptedDate != null);
            filter = filter.And(item => item.BidCompletedDate != null);
            filter = filter.And(item => item.BidUserId == userId2 || item.BidUserId == userId1);
            filter = filter.And(item => item.User1IdRated == 0 || item.User2IdRated == 0);
            var RateTransaction = await _repositoty.GetByFilter(filter);
            return RateTransaction;
        }

    }
}
