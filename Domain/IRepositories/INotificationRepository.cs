using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<List<Notification>> GetNotificationByUserId(int userId);
        Task<int> GetNotificationMax();
        Task<int> NumberNotification(int userId);
        Task<bool> AddNotification(Notification notification);
        Task UpdateNotification(int notificationId);
        Task DeleteNotification(int notificationId);
        Task MarkToReadAllNotification(int userId);
        Task DeleteAllNotification(int userId);
    }
}
