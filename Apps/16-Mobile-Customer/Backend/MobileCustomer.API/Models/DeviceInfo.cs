using System.ComponentModel.DataAnnotations;

namespace MobileCustomer.API.Models
{
    public class DeviceInfo
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [MaxLength(200)]
        public string DeviceId { get; set; }
        
        [MaxLength(100)]
        public string DeviceName { get; set; }
        
        [MaxLength(50)]
        public string Platform { get; set; } // iOS, Android
        
        [MaxLength(50)]
        public string OsVersion { get; set; }
        
        [MaxLength(50)]
        public string AppVersion { get; set; }
        
        [MaxLength(500)]
        public string PushToken { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime LastActiveAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}