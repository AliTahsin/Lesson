namespace Notification.API.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string SenderName { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ActionUrl { get; set; }
        public int? RelatedId { get; set; }
    }

    public class CreateNotificationDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
        public int? RecipientId { get; set; }
        public string RecipientType { get; set; }
        public int? HotelId { get; set; }
        public string ActionUrl { get; set; }
        public int? RelatedId { get; set; }
        public bool SendPush { get; set; } = true;
    }

    public class MarkAsReadDto
    {
        public int NotificationId { get; set; }
    }

    public class UnreadCountDto
    {
        public int Count { get; set; }
    }
}