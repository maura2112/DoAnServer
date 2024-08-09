using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        async Task<bool> INotificationRepository.AddNotification(Notification notification)
        {

            await _context.AddAsync(notification);
            int result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }           
        }

       async Task INotificationRepository.DeleteAllNotification(int userId)
        {
            var notification = await _context.Notifications.Where(x => x.RecieveId == userId).ToListAsync();
            if (notification.Count == 0)
            {
                return;
            }
            _context.Notifications.RemoveRange(notification);
            await _context.SaveChangesAsync();
        }

        async Task INotificationRepository.DeleteNotification(int notificationId)
        {
            var notification = await _context.Notifications.
              Where(x => x.NotificationId == notificationId).FirstOrDefaultAsync();
            if (notification == null)
            {
                return;
            }
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }

        async Task<List<Notification>> INotificationRepository.GetNotificationByUserId(int userId)
        {
            List<Notification> list = await _context.Notifications
             .Where(x => x.RecieveId == userId)
                 .OrderByDescending(x => x.Datetime)
                     .Take(50)
             .ToListAsync();

            return list;
        }

       async Task<int> INotificationRepository.GetNotificationMax()
        {
            var maxNotiId = await _context.Notifications.MaxAsync(x => x.NotificationId);
            return maxNotiId;
        }

        async Task INotificationRepository.MarkToReadAllNotification(int userId)
        {

            var notification = await _context.Notifications.Where(x => x.RecieveId == userId).ToListAsync();

            notification.ForEach(c => { c.IsRead = 1; });

            _context.UpdateRange(notification);
            await _context.SaveChangesAsync();
            return;
        }

        async Task<int> INotificationRepository.NumberNotification(int userId)
        {
            int x = await _context.Notifications.Where(x => x.RecieveId == userId  && x.IsRead == 0 ).CountAsync();
            return x;
        }

        async Task INotificationRepository.UpdateNotification(int notificationId)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.NotificationId == notificationId);
            notification.IsRead = 1;
            await _context.SaveChangesAsync();
        }
    }
}
