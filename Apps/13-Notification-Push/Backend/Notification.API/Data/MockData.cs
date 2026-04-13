using Notification.API.Models;

namespace Notification.API.Data
{
    public static class MockData
    {
        private static readonly Random _random = new();

        public static List<Notification> GetNotifications()
        {
            var notifications = new List<Notification>();
            var types = new[] { "Info", "Success", "Warning", "Error", "Promo" };
            var categories = new[] { "Reservation", "Payment", "Housekeeping", "Promo", "System" };
            
            for (int i = 1; i <= 100; i++)
            {
                var isRead = _random.Next(0, 10) > 6;
                var daysAgo = _random.Next(0, 30);
                
                notifications.Add(new Notification
                {
                    Id = i,
                    Title = GetRandomTitle(categories[_random.Next(categories.Length)]),
                    Body = $"This is notification number {i}. This is a sample notification body.",
                    Type = types[_random.Next(types.Length)],
                    Category = categories[_random.Next(categories.Length)],
                    SenderName = "System",
                    RecipientId = _random.Next(1, 10),
                    RecipientType = _random.Next(0, 10) > 8 ? "All" : "User",
                    IsRead = isRead,
                    ReadAt = isRead ? DateTime.Now.AddDays(-_random.Next(1, daysAgo)) : null,
                    SentAt = DateTime.Now.AddDays(-daysAgo),
                    CreatedAt = DateTime.Now.AddDays(-daysAgo),
                    ActionUrl = _random.Next(0, 10) > 7 ? "/reservations/123" : null,
                    RelatedId = _random.Next(0, 10) > 7 ? _random.Next(1, 500) : null
                });
            }
            
            return notifications.OrderByDescending(n => n.CreatedAt).ToList();
        }

        public static List<PushSubscription> GetSubscriptions()
        {
            var subscriptions = new List<PushSubscription>();
            var deviceTypes = new[] { "iOS", "Android", "Web" };
            
            for (int i = 1; i <= 25; i++)
            {
                subscriptions.Add(new PushSubscription
                {
                    Id = i,
                    UserId = _random.Next(1, 10),
                    Endpoint = $"https://fcm.googleapis.com/fcm/send/endpoint-{i}",
                    P256dh = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                    Auth = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                    DeviceType = deviceTypes[_random.Next(deviceTypes.Length)],
                    DeviceName = $"Device {i}",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-_random.Next(1, 60))
                });
            }
            
            return subscriptions;
        }

        private static string GetRandomTitle(string category)
        {
            return category switch
            {
                "Reservation" => new[] { "Yeni Rezervasyon", "Rezervasyon Onayı", "Rezervasyon İptali" }[_random.Next(3)],
                "Payment" => new[] { "Ödeme Başarılı", "Ödeme Onayı", "Ödeme Hatası" }[_random.Next(3)],
                "Housekeeping" => new[] { "Oda Temizliği", "Görev Atandı", "Arıza Bildirimi" }[_random.Next(3)],
                "Promo" => new[] { "Özel Teklif", "İndirim Fırsatı", "Hediye Puan" }[_random.Next(3)],
                _ => new[] { "Sistem Güncellemesi", "Bilgilendirme", "Hatırlatma" }[_random.Next(3)]
            };
        }
    }
}