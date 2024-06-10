using Application.DTOs;
using Application.IServices;
using Application.Services;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using static API.Common.Url;

namespace API.Controllers
{
    
    public class BidController : ApiControllerBase
    {
        private readonly IBidService _bidService;
        private readonly IBidRepository _bidRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUserService _currentUserService;


        public BidController(IBidService bidService, ICurrentUserService currentUserService, IBidRepository bidRepository, ICurrentUserService currentUser, IProjectRepository projectRepository)
        {
            _bidService = bidService;
            _bidRepository = bidRepository;
            _currentUserService = currentUser;
            _projectRepository = projectRepository;
        }

        [HttpGet]
        [Route(Common.Url.Bid.GetBiddingListByUserId)]
        public async Task<IActionResult> GetListByUserId([FromQuery] BidSearchDTO bids)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            Expression<Func<Domain.Entities.Bid, bool>> filter = null;
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
            Expression<Func<Domain.Entities.Bid, bool>> filter = null;
            if (bids != null)
            {
                filter = item => item.ProjectId == bids.ProjectId;
            }
            return Ok(await _bidService.GetWithFilter(filter, bids.PageIndex, bids.PageSize));
        }

        [HttpPost]
        [Route(Common.Url.Bid.Bidding)]
        public async Task<IActionResult> Bidding(BiddingDTO DTOs, CancellationToken token)
        {
            var userId = _currentUserService.UserId;
            var fetchedProject = await _projectRepository.GetByIdAsync(DTOs.ProjectId);
            if(fetchedProject == null)
            {
                return NotFound(new {message = "Không tìm thấy dự án!"});
            }
            else
            {
                bool isBidding = await _bidRepository.CheckBidding(userId, DTOs.ProjectId);
                if (isBidding)
                {
                    return BadRequest(new { message = "Bạn đã đấu thầu dự án này" });
                }
                else
                {
                    var bid = await _bidService.Add(DTOs);
                    return Ok(new
                    {
                        success = true,
                        message = "Bạn vừa tạo đấu thầu thành công",
                        data = bid
                    });
                }
            }
            
        }

        //[HttpDelete]
        //[Route(Common.Url.Bid.Delete)]
        //public async Task<IActionResult> DeleteBidding(BidDTO DTOs, CancellationToken token)
        //{
        //    //var userId = _currentUserService.UserId;
        //    if (!ModelState.IsValid)
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        //    }
        //    //DTOs.UserId = userId;
        //    await _bidService.Delete((int)DTOs.Id);
        //    return NoContent();
        //}
        [HttpPut]
        [Route(Common.Url.Bid.Update)]
        public async Task<IActionResult> UpdateBidding(UpdateBidDTO DTOs, CancellationToken token)
        {
            //var userId = _currentUserService.UserId;
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            var bidDTO = await _bidRepository.GetByIdAsync(DTOs.Id);
            if(bidDTO == null)
            {
                return NotFound(new {message="Không tìm thấy đấu thầu!"});
            }
            //DTOs.UserId = userId;
            else
            {
                var bid = await _bidService.Update(DTOs);
                return Ok(new
                {
                    success = true,
                    message = "Bạn vừa cập nhật đấu thầu thành công",
                    data = bid
                });
            }
            
        }
        [HttpPut]
        [Route(Common.Url.Bid.AcceptBidding)]
        public async Task<IActionResult> AcceptBidding(BidAccepted DTOs, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            var bidDTO = await _bidRepository.GetByIdAsync(DTOs.Id);
            if (bidDTO == null)
            {
                return NotFound();
            }
            else
            {
                var bid = await _bidService.AcceptBidding(DTOs.Id);
                return Ok(new
                {
                    success = true,
                    message = "Chấp nhận đấu thầu",
                    data = bid
                });
            }
        }
    }
}
