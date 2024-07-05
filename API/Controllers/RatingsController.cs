using Application.DTOs;
using Application.IServices;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RatingsController : ApiControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IRateTransactionService _transactionService;
        private readonly IRatingService _ratingService;
        public RatingsController(ICurrentUserService currentUserService, IRateTransactionService transactionService, IRatingService ratingService)
        {
            _currentUserService = currentUserService;
            _transactionService = transactionService;
            _ratingService = ratingService;
        }
        [HttpGet]
        [Route(Common.Url.Rating.Rate)]
        public async Task<ActionResult> Rating(RatingDTO rating)
        {
            var userId =  _currentUserService.UserId;
            if(userId == rating.RateToUserId)
            {
                return BadRequest("Bạn không thể đánh giá chính mình");
            }
            var ratingTrasaction = _transactionService.GetRateTransactionByUsers(rating.RateToUserId, userId);
            if(ratingTrasaction == null)
            {
                return NotFound("Bạn không thể đánh giá người này");
            }
            rating.UserId = userId;
            var dto = _ratingService.CreateRating(rating);
            return Ok(dto);
        }
    }
}
