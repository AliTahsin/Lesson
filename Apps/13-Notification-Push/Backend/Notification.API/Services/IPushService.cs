namespace Notification.API.Services
{
    public interface IPushService
    {
        Task<bool> SendToUserAsync(int userId, string title, string body, object data = null);
        Task<bool> SendToRoleAsync(string role, string title, string body, object data = null);
        Task<bool> SendToHotelAsync(int hotelId, string title, string body, object data = null);
        Task<bool> SendToAllAsync(string title, string body, object data = null);
        Task<bool> SubscribeAsync(int userId, string endpoint, string p256dh, string auth, string deviceType);
        Task<bool> UnsubscribeAsync(string endpoint);
    }
}