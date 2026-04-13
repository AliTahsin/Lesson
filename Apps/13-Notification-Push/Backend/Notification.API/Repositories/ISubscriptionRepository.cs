using Notification.API.Models;

namespace Notification.API.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<PushSubscription> GetByIdAsync(int id);
        Task<PushSubscription> GetByEndpointAsync(string endpoint);
        Task<List<PushSubscription>> GetByUserAsync(int userId);
        Task<PushSubscription> AddAsync(PushSubscription subscription);
        Task<PushSubscription> UpdateAsync(PushSubscription subscription);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByEndpointAsync(string endpoint);
        Task<List<PushSubscription>> GetActiveSubscriptionsAsync();
    }
}