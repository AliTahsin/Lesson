using Notification.API.Models;

namespace Notification.API.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> GetByIdAsync(int id);
        Task<List<Notification>> GetByUserAsync(int userId);
        Task<List<Notification>> GetByHotelAsync(int hotelId);
        Task<List<Notification>> GetUnreadByUserAsync(int userId);
        Task<Notification> AddAsync(Notification notification);
        Task<Notification> UpdateAsync(Notification notification);
        Task<bool> MarkAsReadAsync(int id, int userId);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<bool> DeleteAsync(int id);
    }
}