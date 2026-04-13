using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Token { get; set; }
        
        [MaxLength(50)]
        public string DeviceId { get; set; }
        
        [MaxLength(200)]
        public string DeviceName { get; set; }
        
        [MaxLength(50)]
        public string IpAddress { get; set; }
        
        [MaxLength(500)]
        public string UserAgent { get; set; }
        
        public DateTime ExpiryDate { get; set; }
        
        public bool IsRevoked { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}