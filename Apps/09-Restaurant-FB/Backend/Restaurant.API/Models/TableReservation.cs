using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.API.Models
{
    public class TableReservation
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string ReservationNumber { get; set; }
        
        [Required]
        public int RestaurantId { get; set; }
        
        public int? TableId { get; set; }
        
        [MaxLength(50)]
        public string TableNumber { get; set; }
        
        public int? CustomerId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; }
        
        [MaxLength(100)]
        public string CustomerEmail { get; set; }
        
        [MaxLength(20)]
        public string CustomerPhone { get; set; }
        
        public int GuestCount { get; set; }
        
        public DateTime ReservationDate { get; set; }
        
        [MaxLength(10)]
        public string ReservationTime { get; set; }
        
        public int DurationMinutes { get; set; } = 120;
        
        [MaxLength(500)]
        public string SpecialRequests { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Pending, Confirmed, Arrived, Cancelled, NoShow
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? ConfirmedAt { get; set; }
        
        public DateTime? ArrivedAt { get; set; }
        
        public DateTime? CancelledAt { get; set; }
    }
}