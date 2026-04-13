namespace Notification.API.DTOs
{
    public class PushSubscriptionDto
    {
        public string Endpoint { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }
        public string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string OsVersion { get; set; }
        public string AppVersion { get; set; }
    }

    public class PushSendDto
    {
        public int? UserId { get; set; }
        public string Role { get; set; }
        public int? HotelId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public object Data { get; set; }
    }
}