using System.ComponentModel.DataAnnotations;

namespace Notification.API.Models
{
    public class NotificationLog
    {
        [Key]
        public int Id { get; set; }
        
        public int NotificationId { get; set; }
        
        public int RecipientId { get; set; }
        
        [MaxLength(200)]
        public string RecipientEmail { get; set; }
        
        [MaxLength(50)]
        public string Channel { get; set; } // InApp, Email, SMS, Push
        
        public bool IsSuccess { get; set; }
        
        [MaxLength(500)]
        public string ErrorMessage { get; set; }
        
        public DateTime SentAt { get; set; }
    }
}