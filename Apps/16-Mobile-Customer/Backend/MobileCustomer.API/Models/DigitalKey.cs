using System.ComponentModel.DataAnnotations;

namespace MobileCustomer.API.Models
{
    public class DigitalKey
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ReservationId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int RoomId { get; set; }
        
        [MaxLength(50)]
        public string RoomNumber { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string KeyCode { get; set; }
        
        [MaxLength(500)]
        public string QrCode { get; set; }
        
        public DateTime ValidFrom { get; set; }
        
        public DateTime ValidUntil { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsUsed { get; set; }
        
        public DateTime? UsedAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}