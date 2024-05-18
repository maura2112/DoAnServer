using Application.DTOs;
using Application.IServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    
    public class BidController : ApiControllerBase
    {
        private readonly IBidService _bidService;
        public BidController(IBidService bidService)
        {
            _bidService = bidService;
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
        public async Task<IActionResult> AddAsync([FromQuery] BidDTO DTOs, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            await _bidService.Add(DTOs);
            return NoContent();
        }
    }
}
