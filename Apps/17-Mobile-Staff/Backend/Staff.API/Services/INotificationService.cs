using Staff.API.Models;

namespace Staff.API.Services
{
    public interface INotificationService
    {
        Task SendTaskNotification(int staffId, int taskId, string taskType);
        Task SendIssueNotification(int staffId, int issueId, string category);
        Task SendCriticalIssueNotification(int hotelId, MaintenanceIssue issue);
        Task<List<Notification>> GetStaffNotificationsAsync(int staffId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<int> GetUnreadCountAsync(int staffId);
    }
}