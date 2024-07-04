using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetNotificationByUserId(int userId);
        Task<int> NumberNotification(int userId);

        Task<bool> AddNotification(NotificationDto notification);
        Task UpdateNotification(int notificationId);
        Task DeleteNotification(int notificationId);
        Task MarkToReadAllNotification(int userId);
        Task DeleteAllNotification(int userId);
    }
}
