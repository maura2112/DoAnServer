using Application.DTOs;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IMapper mapper, INotificationRepository notificationRepository)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

       async Task<bool> INotificationService.AddNotification(NotificationDto notification)
        {

            Notification noti = _mapper.Map<Notification>(notification);
           bool x= await _notificationRepository.AddNotification(noti);
            return x;
        }

        async Task INotificationService.DeleteAllNotification(int userId)
        {
            await _notificationRepository.DeleteAllNotification(userId);
        }

        async Task INotificationService.DeleteNotification(int notificationId)
        {
            await _notificationRepository.DeleteNotification(notificationId);
        }

        async Task<List<NotificationDto>> INotificationService.GetNotificationByUserId(int userId)
        {
            List<Notification> list =await _notificationRepository.GetNotificationByUserId(userId);
            List<NotificationDto> lists = _mapper.Map<List<NotificationDto>>(list);
            return lists;
        }

        async Task INotificationService.MarkToReadAllNotification(int userId)
        {
             await _notificationRepository.MarkToReadAllNotification(userId);
        }

        async Task<int> INotificationService.NumberNotification(int userId)
        {
            int x =await _notificationRepository.NumberNotification(userId);
            return x;
        }

        async Task INotificationService.UpdateNotification(int notificationId)
        {
            await _notificationRepository.UpdateNotification(notificationId);
        }
    }
}
