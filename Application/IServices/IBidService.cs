using Application.DTOs;
using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IBidService
    {
        //list by user, list by project
        Task<Pagination<BidDTO>> GetWithFilter(Expression<Func<Bid, bool>> filter, int pageIndex, int pageSize);

        //Task<BidDTO> GetDetailBidById(int id);
        Task<int> Add(BidDTO request);

        Task<int> Update(BidDTO request);
        Task<int> Delete(int id);

    }
}
