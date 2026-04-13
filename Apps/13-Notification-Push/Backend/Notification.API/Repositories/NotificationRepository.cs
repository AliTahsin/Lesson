using Notification.API.Models;
using Notification.API.Data;

namespace Notification.API.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly List<Notification> _notifications;

        public NotificationRepository()
        {
            _notifications = MockData.GetNotifications();
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await Task.FromResult(_notifications.FirstOrDefault(n => n.Id == id));
        }

        public async Task<List<Notification>> GetByUserAsync(int userId)
        {
            return await Task.FromResult(_notifications
                .Where(n => n.RecipientId == userId || n.RecipientType == "All")
                .OrderByDescending(n => n.CreatedAt)
                .ToList());
        }

        public async Task<List<Notification>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_notifications
                .Where(n => n.HotelId == hotelId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList());
        }

        public async Task<List<Notification>> GetUnreadByUserAsync(int userId)
        {
            return await Task.FromResult(_notifications
                .Where(n => (n.RecipientId == userId || n.RecipientType == "All") && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToList());
        }

        public async Task<Notification> AddAsync(Notification notification)
        {
            notification.Id = _notifications.Max(n => n.Id) + 1;
            notification.CreatedAt = DateTime.Now;
            notification.IsRead = false;
            _notifications.Add(notification);
            return await Task.FromResult(notification);
        }

        public async Task<Notification> UpdateAsync(Notification notification)
        {
            var existing = await GetByIdAsync(notification.Id);
            if (existing != null)
            {
                var index = _notifications.IndexOf(existing);
                _notifications[index] = notification;
            }
            return await Task.FromResult(notification);
        }

        public async Task<bool> MarkAsReadAsync(int id, int userId)
        {
            var notification = await GetByIdAsync(id);
            if (notification != null && (notification.RecipientId == userId || notification.RecipientType == "All"))
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var userNotifications = _notifications
                .Where(n => (n.RecipientId == userId || n.RecipientType == "All") && !n.IsRead);
            
            foreach (var notification in userNotifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.Now;
            }
            return await Task.FromResult(true);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await Task.FromResult(_notifications
                .Count(n => (n.RecipientId == userId || n.RecipientType == "All") && !n.IsRead));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var notification = await GetByIdAsync(id);
            if (notification != null)
            {
                _notifications.Remove(notification);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}