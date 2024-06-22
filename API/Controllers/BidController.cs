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
        private readonly IAppUserRepository _appUserRepository;


        public BidController(IBidService bidService, IBidRepository bidRepository, ICurrentUserService currentUser, IProjectRepository projectRepository, IAppUserRepository appUserRepository)
        {
            _bidService = bidService;
            _bidRepository = bidRepository;
            _currentUserService = currentUser;
            _projectRepository = projectRepository;
            _appUserRepository = appUserRepository;
        }

        [HttpGet]
        [Route(Common.Url.Bid.GetBiddingListByUserId)]
        public async Task<IActionResult> GetListByUserId([FromQuery] BidSearchDTO bids)
        {
            try
            {
                var fetchedUser = await _appUserRepository.GetByIdAsync(bids.UserId);
                if (fetchedUser == null)
                {
                    return NotFound(new { message = "Người dùng không tồn tại!" });
                }

                Expression<Func<Domain.Entities.Bid, bool>> filter = null;
                if (bids != null)
                {
                    filter = item => item.UserId == bids.UserId;
                }

                var result = await _bidService.GetWithFilter(filter, bids.PageIndex, bids.PageSize);

                string msg = null;
                if (result.TotalItemsCount <= 0)
                {
                    msg = "Bạn chưa có đấu thầu nào!";
                };
                return Ok(new
                {
                    success = true,
                    message = msg,
                    data = result
                });
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
            }
        }

        [HttpGet]
        [Route(Common.Url.Bid.GetBiddingListByProjectId)]
        public async Task<IActionResult> GetListByProjectId([FromQuery] BidListDTO bids)
        {
            try
            {
                var fetchedProject = await _projectRepository.GetByIdAsync(bids.ProjectId);
                if (fetchedProject == null)
                {
                    return NotFound(new { message = "Không tìm thấy dự án!" });

                }
                Expression<Func<Domain.Entities.Bid, bool>> filter = null;
                if (bids != null && bids.ProjectId > 0)
                {
                    filter = item => item.ProjectId == bids.ProjectId;
                }

                var result = await _bidService.GetWithFilter(filter, bids.PageIndex, bids.PageSize);
                string msg = null;
                if (result.TotalItemsCount <= 0)
                {
                    msg = "Chưa có đấu thầu nào cho dự án này!";
                };
                return Ok(new
                {
                    success = true,
                    message = msg,
                    data = result
                });
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
            }
        }

        [HttpPost]
        [Route(Common.Url.Bid.Bidding)]
        public async Task<IActionResult> Bidding(BiddingDTO DTOs, CancellationToken token)
        {
            var userId = _currentUserService.UserId;
            if (userId <=0)
            {
                return Unauthorized(); // Chuyển về HTTP 401 Unauthorized nếu chưa đăng nhập
            }

            var fetchedProject = await _projectRepository.GetByIdAsync(DTOs.ProjectId);
            if (fetchedProject == null)
            {
                return NotFound(new { message = "Không tìm thấy dự án!" });

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
                    try
                    {
                        var bid = await _bidService.Add(DTOs);
                        var response = new BidResponseDTO
                        {
                            Success = true,
                            Message = "Bạn vừa tạo đấu thầu thành công",
                            Data = bid
                        };
                        return Ok(new
                        {
                            success = true,
                            message = "Bạn vừa tạo đấu thầu thành công",
                            data = bid
                        });
                    }
                    catch (Exception ex)
                    {
                        // Log the exception here if needed
                        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
                    }
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
                return BadRequest(ModelState);
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
                return BadRequest(ModelState);
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
