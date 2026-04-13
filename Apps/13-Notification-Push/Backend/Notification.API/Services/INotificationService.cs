using Notification.API.DTOs;

namespace Notification.API.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetUserNotificationsAsync(int userId);
        Task<List<NotificationDto>> GetUnreadNotificationsAsync(int userId);
        Task<NotificationDto> GetNotificationByIdAsync(int id);
        Task<NotificationDto> SendNotificationAsync(CreateNotificationDto dto);
        Task<bool> MarkAsReadAsync(int id, int userId);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<bool> DeleteNotificationAsync(int id, int userId);
        Task<NotificationDto> SendReservationNotificationAsync(int userId, int reservationId, string status);
        Task<NotificationDto> SendPaymentNotificationAsync(int userId, int paymentId, string status);
        Task<NotificationDto> SendPromoNotificationAsync(int hotelId, string title, string message);
    }
}