using Notification.API.Models;
using Notification.API.Data;

namespace Notification.API.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly List<PushSubscription> _subscriptions;

        public SubscriptionRepository()
        {
            _subscriptions = MockData.GetSubscriptions();
        }

        public async Task<PushSubscription> GetByIdAsync(int id)
        {
            return await Task.FromResult(_subscriptions.FirstOrDefault(s => s.Id == id));
        }

        public async Task<PushSubscription> GetByEndpointAsync(string endpoint)
        {
            return await Task.FromResult(_subscriptions.FirstOrDefault(s => s.Endpoint == endpoint));
        }

        public async Task<List<PushSubscription>> GetByUserAsync(int userId)
        {
            return await Task.FromResult(_subscriptions.Where(s => s.UserId == userId && s.IsActive).ToList());
        }

        public async Task<PushSubscription> AddAsync(PushSubscription subscription)
        {
            subscription.Id = _subscriptions.Max(s => s.Id) + 1;
            subscription.CreatedAt = DateTime.Now;
            subscription.IsActive = true;
            _subscriptions.Add(subscription);
            return await Task.FromResult(subscription);
        }

        public async Task<PushSubscription> UpdateAsync(PushSubscription subscription)
        {
            var existing = await GetByIdAsync(subscription.Id);
            if (existing != null)
            {
                var index = _subscriptions.IndexOf(existing);
                subscription.UpdatedAt = DateTime.Now;
                _subscriptions[index] = subscription;
            }
            return await Task.FromResult(subscription);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subscription = await GetByIdAsync(id);
            if (subscription != null)
            {
                _subscriptions.Remove(subscription);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteByEndpointAsync(string endpoint)
        {
            var subscription = await GetByEndpointAsync(endpoint);
            if (subscription != null)
            {
                _subscriptions.Remove(subscription);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<PushSubscription>> GetActiveSubscriptionsAsync()
        {
            return await Task.FromResult(_subscriptions.Where(s => s.IsActive).ToList());
        }
    }
}