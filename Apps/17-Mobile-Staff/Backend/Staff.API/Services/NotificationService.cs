using Staff.API.Models;
using Staff.API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Staff.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly List<Notification> _notifications;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IHubContext<SignalRHub> hubContext, ILogger<NotificationService> logger)
        {
            _notifications = new List<Notification>();
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task SendTaskNotification(int staffId, int taskId, string taskType)
        {
            var notification = new Notification
            {
                Id = _notifications.Count + 1,
                StaffId = staffId,
                Title = "Yeni Görev",
                Body = $"Yeni bir {GetTaskTypeText(taskType)} görevi size atandı.",
                Type = "Task",
                RelatedId = taskId,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            _notifications.Add(notification);
            await _hubContext.Clients.User(staffId.ToString()).SendAsync("NewNotification", notification);
        }

        public async Task SendIssueNotification(int staffId, int issueId, string category)
        {
            var notification = new Notification
            {
                Id = _notifications.Count + 1,
                StaffId = staffId,
                Title = "Yeni Arıza",
                Body = $"Yeni bir {category} arızası size atandı.",
                Type = "Issue",
                RelatedId = issueId,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            _notifications.Add(notification);
            await _hubContext.Clients.User(staffId.ToString()).SendAsync("NewNotification", notification);
        }

        public async Task SendCriticalIssueNotification(int hotelId, MaintenanceIssue issue)
        {
            var notification = new Notification
            {
                Id = _notifications.Count + 1,
                StaffId = 0,
                Title = "Kritik Arıza",
                Body = $"Kritik arıza bildirildi: {issue.Description}",
                Type = "Critical",
                RelatedId = issue.Id,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            await _hubContext.Clients.Group($"hotel-{hotelId}").SendAsync("CriticalAlert", notification);
        }

        public async Task<List<Notification>> GetStaffNotificationsAsync(int staffId)
        {
            return await Task.FromResult(_notifications
                .Where(n => n.StaffId == staffId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList());
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<int> GetUnreadCountAsync(int staffId)
        {
            return await Task.FromResult(_notifications.Count(n => n.StaffId == staffId && !n.IsRead));
        }

        private string GetTaskTypeText(string taskType)
        {
            return taskType switch
            {
                "CheckOut" => "check-out",
                "StayOver" => "stay-over",
                "DeepClean" => "deep clean",
                "Inspection" => "inspection",
                _ => taskType
            };
        }
    }
}