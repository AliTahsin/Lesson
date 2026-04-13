using System.ComponentModel.DataAnnotations;

namespace Notification.API.Models
{
    public class PushSubscription
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Endpoint { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string P256dh { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Auth { get; set; }
        
        [MaxLength(50)]
        public string DeviceType { get; set; } // Web, iOS, Android
        
        [MaxLength(200)]
        public string DeviceName { get; set; }
        
        [MaxLength(50)]
        public string OsVersion { get; set; }
        
        [MaxLength(50)]
        public string AppVersion { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? ExpiresAt { get; set; }
    }
}