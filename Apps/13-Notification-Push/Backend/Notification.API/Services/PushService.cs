using Notification.API.Repositories;
using System.Text;
using System.Text.Json;

namespace Notification.API.Services
{
    public class PushService : IPushService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger<PushService> _logger;

        public PushService(
            ISubscriptionRepository subscriptionRepository,
            ILogger<PushService> logger)
        {
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
        }

        public async Task<bool> SendToUserAsync(int userId, string title, string body, object data = null)
        {
            var subscriptions = await _subscriptionRepository.GetByUserAsync(userId);
            var success = true;

            foreach (var sub in subscriptions)
            {
                var result = await SendPushNotification(sub, title, body, data);
                if (!result)
                {
                    success = false;
                    // If subscription is invalid, remove it
                    if (result == false)
                    {
                        await _subscriptionRepository.DeleteAsync(sub.Id);
                    }
                }
            }

            return success;
        }

        public async Task<bool> SendToRoleAsync(string role, string title, string body, object data = null)
        {
            // Simulate sending to role
            _logger.LogInformation($"Sending push to role {role}: {title}");
            await Task.Delay(100);
            return true;
        }

        public async Task<bool> SendToHotelAsync(int hotelId, string title, string body, object data = null)
        {
            // Simulate sending to hotel
            _logger.LogInformation($"Sending push to hotel {hotelId}: {title}");
            await Task.Delay(100);
            return true;
        }

        public async Task<bool> SendToAllAsync(string title, string body, object data = null)
        {
            var subscriptions = await _subscriptionRepository.GetActiveSubscriptionsAsync();
            
            foreach (var sub in subscriptions)
            {
                await SendPushNotification(sub, title, body, data);
            }
            
            return true;
        }

        public async Task<bool> SubscribeAsync(int userId, string endpoint, string p256dh, string auth, string deviceType)
        {
            // Check if subscription already exists
            var existing = await _subscriptionRepository.GetByEndpointAsync(endpoint);
            if (existing != null)
            {
                existing.IsActive = true;
                existing.UpdatedAt = DateTime.Now;
                await _subscriptionRepository.UpdateAsync(existing);
                return true;
            }

            var subscription = new Models.PushSubscription
            {
                UserId = userId,
                Endpoint = endpoint,
                P256dh = p256dh,
                Auth = auth,
                DeviceType = deviceType,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _subscriptionRepository.AddAsync(subscription);
            return true;
        }

        public async Task<bool> UnsubscribeAsync(string endpoint)
        {
            return await _subscriptionRepository.DeleteByEndpointAsync(endpoint);
        }

        private async Task<bool> SendPushNotification(Models.PushSubscription subscription, string title, string body, object data)
        {
            try
            {
                // Simulate sending push notification via FCM/APNS
                _logger.LogInformation($"Sending push to {subscription.DeviceType}: {title}");
                
                var payload = new
                {
                    title,
                    body,
                    data,
                    timestamp = DateTime.Now
                };
                
                // In real implementation, use Firebase Admin SDK or web-push
                await Task.Delay(50);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send push notification");
                return false;
            }
        }
    }
}