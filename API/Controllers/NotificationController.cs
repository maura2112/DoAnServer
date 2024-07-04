using API.Hubs;
using Application.DTOs;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static API.Common.Url;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        public NotificationController(IMapper mapper, IHubContext<ChatHub> chatHubContext, INotificationService notificationService,
            INotificationRepository notificationRepository)
        {
            _mapper = mapper;
            _chatHubContext = chatHubContext;
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
        }

        [HttpGet("GetNotificationByUserId/{userId}")]
        public async Task<IActionResult> GetNotificationByUserId(int userId)
        {
            List<NotificationDto> lists =await _notificationService.GetNotificationByUserId(userId);
            return Ok(lists);
        }

        [HttpGet("numberNotification/{userId}")]
        public async Task<IActionResult> NumberNotification(int userId)
        {
            int x = await _notificationService.NumberNotification(userId);
            return Ok(x);
        }

        [HttpPut("update/{notificationId}")]
        public async Task<IActionResult> UpdateNotification(int notificationId)
        {
            await _notificationService.UpdateNotification(notificationId);
            return Ok();
        }

        [HttpDelete("delete/{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            await _notificationService.DeleteNotification(notificationId);
            return Ok();
        }
        [HttpPut("markToReadAll/{userId}")]
        public async Task<IActionResult> MarkToReadAllNotification(int userId)
        {
            await _notificationService.MarkToReadAllNotification(userId);
            return Ok();
        }

        [HttpDelete("deleteAll/{userId}")]
        public async Task<IActionResult> DeleteAllNotification(int userId)
        {
            await _notificationService.DeleteAllNotification(userId);
            return Ok();

        }
    }
}
