using Application.DTOs;
using Application.IServices;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    
    public class BidController : ApiControllerBase
    {
        private readonly IBidService _bidService;
        private readonly ICurrentUserService _currentUserService;
        public BidController(IBidService bidService, ICurrentUserService currentUserService)
        {
            _bidService = bidService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [Route(Common.Url.Bid.GetBiddingListByUserId)]
        public async Task<IActionResult> GetListByUserId([FromQuery] BidSearchDTO bids)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            Expression<Func<Bid, bool>> filter = null;
            if (bids != null)
            {
                filter = item => item.UserId == bids.UserId;
            }
            return Ok(await _bidService.GetWithFilter(filter, bids.PageIndex, bids.PageSize));
        }

        [HttpGet]
        [Route(Common.Url.Bid.GetBiddingListByProjectId)]
        public async Task<IActionResult> GetListByProjectId([FromQuery] BidListDTO bids)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            Expression<Func<Bid, bool>> filter = null;
            if (bids != null)
            {
                filter = item => item.ProjectId == bids.ProjectId;
            }
            return Ok(await _bidService.GetWithFilter(filter, bids.PageIndex, bids.PageSize));
        }

        [HttpPost]
        [Route(Common.Url.Bid.Bidding)]
        public async Task<IActionResult> AddAsync( BidDTO DTOs, CancellationToken token)
        {
            var userId = _currentUserService.UserId;
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            DTOs.UserId = userId;
            await _bidService.Add(DTOs);
            return NoContent();
        }
    }
}
