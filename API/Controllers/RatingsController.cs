using API.Hubs;
using Application.DTOs;
using Application.IServices;
using Application.Services;
using Domain.IRepositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class RatingsController : ApiControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationService _notificationService;
        private readonly IRateTransactionService _transactionService;
        private readonly IRateTransactionRepository _rateTransactionRepository;
        private readonly IRatingService _ratingService;
        private readonly IProjectService _projectService;
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _chatHubContext;
        public RatingsController(ICurrentUserService currentUserService, IRateTransactionService transactionService, IRatingService ratingService, IProjectService projectService, IRateTransactionRepository rateTransactionRepository, ApplicationDbContext context, IHubContext<ChatHub> chatHubContext, INotificationService notificationService, INotificationRepository notificationRepository)
        {
            _currentUserService = currentUserService;
            _transactionService = transactionService;
            _ratingService = ratingService;
            _projectService = projectService;
            _rateTransactionRepository = rateTransactionRepository;
            _context = context;
            _chatHubContext = chatHubContext;
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
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
                return NotFound("Bạn không thể đánh giá người này-");
            }
            if(ratingTrasaction.User1IdRated == userId || ratingTrasaction.User2IdRated == userId)
            {
                return BadRequest("Bạn không thể đánh giá người này");
            }
            if(ratingTrasaction.User1IdRated != 0) {
                ratingTrasaction.User2IdRated = userId;
            }else
            {
                ratingTrasaction.User1IdRated = userId;
            }
            var maxNotiId = await _notificationRepository.GetNotificationMax() + 1;
            NotificationDto notificationDto = new NotificationDto()
            {
                NotificationId = maxNotiId,
                SendId = userId,
                SendUserName = _currentUserService.Name,
                ProjectName = "",//k can cx dc
                RecieveId = rating.RateToUserId,
                Description = "đã đánh giá bạn !",
                Datetime = DateTime.Now,
                NotificationType = 1,
                IsRead = 0,
                Link = "profile/" + rating.RateToUserId
            };
            bool x = await _notificationService.AddNotification(notificationDto);
            if (x)
            {
                var hubConnections = await _context.HubConnections
                            .Where(con => con.userId == rating.RateToUserId).ToListAsync();
                foreach (var hubConnection in hubConnections)
                {
                    await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedNotification", notificationDto);
                }
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
