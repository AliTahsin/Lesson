using System.ComponentModel.DataAnnotations;

namespace Notification.API.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Body { get; set; }
        
        [MaxLength(500)]
        public string ImageUrl { get; set; }
        
        [MaxLength(50)]
        public string Type { get; set; } // Info, Success, Warning, Error, Promo
        
        [MaxLength(50)]
        public string Category { get; set; } // Reservation, Payment, Housekeeping, Promo, System
        
        public int? SenderId { get; set; }
        
        [MaxLength(100)]
        public string SenderName { get; set; }
        
        public int? RecipientId { get; set; }
        
        [MaxLength(50)]
        public string RecipientType { get; set; } // User, Role, Hotel, All
        
        public int? HotelId { get; set; }
        
        public bool IsRead { get; set; }
        
        public bool IsSent { get; set; }
        
        public bool IsPushSent { get; set; }
        
        public DateTime? ReadAt { get; set; }
        
        public DateTime SentAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        [MaxLength(500)]
        public string Data { get; set; } // JSON data for additional info
        
        public string ActionUrl { get; set; }
        
        public int? RelatedId { get; set; } // ReservationId, PaymentId, etc.
    }
}