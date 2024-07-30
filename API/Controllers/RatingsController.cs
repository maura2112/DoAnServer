using Application.DTOs;
using Application.IServices;
using Application.Services;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RatingsController : ApiControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IRateTransactionService _transactionService;
        private readonly IRateTransactionRepository _rateTransactionRepository;
        private readonly IRatingService _ratingService;
        private readonly IProjectService _projectService;
        public RatingsController(ICurrentUserService currentUserService, IRateTransactionService transactionService, IRatingService ratingService, IProjectService projectService, IRateTransactionRepository rateTransactionRepository)
        {
            _currentUserService = currentUserService;
            _transactionService = transactionService;
            _ratingService = ratingService;
            _projectService = projectService;
            _rateTransactionRepository = rateTransactionRepository;
        }
        [HttpPost]
        [Route(Common.Url.Rating.Rate)]
        public async Task<ActionResult> Rating(RatingDTO rating)
        {
            var userId =  _currentUserService.UserId;
            if(userId == rating.RateToUserId)
            {
                return BadRequest("Bạn không thể đánh giá chính mình");
            }
            var ratingTrasaction = await _transactionService.GetRateTransactionByUsers(rating.RateToUserId, userId);
            if(ratingTrasaction == null)
            {
                return NotFound("Bạn không thể đánh giá người này");
            }
            if(ratingTrasaction.User1IdRated != 0) {
                ratingTrasaction.User2IdRated = userId;
            }else
            {
                ratingTrasaction.User1IdRated = userId;
            }
            _rateTransactionRepository.Update(ratingTrasaction);
            rating.UserId = userId;
            rating.RateTransactionId = ratingTrasaction.Id;
            var dto = await _ratingService.CreateRating(rating);
            dto.ProjectId= ratingTrasaction.ProjectId;
            var project = await _projectService.GetDetailProjectById((int)dto.ProjectId);
            dto.ProjectName = project.Title;
            dto.SkillOfProject = project.Skill;
            return Ok(dto);
        }
    }
}
