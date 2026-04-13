using System.ComponentModel.DataAnnotations;

namespace Notification.API.Models
{
    public class NotificationTemplate
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Body { get; set; }
        
        [MaxLength(50)]
        public string Type { get; set; }
        
        [MaxLength(50)]
        public string Category { get; set; }
        
        [MaxLength(500)]
        public string ActionUrl { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}