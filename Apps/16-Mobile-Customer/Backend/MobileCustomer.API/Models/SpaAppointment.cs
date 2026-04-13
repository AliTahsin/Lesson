using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileCustomer.API.Models
{
    public class SpaAppointment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string AppointmentNumber { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int ReservationId { get; set; }
        
        [MaxLength(100)]
        public string ServiceName { get; set; }
        
        [MaxLength(50)]
        public string ServiceType { get; set; } // Massage, Facial, Sauna, etc.
        
        public int DurationMinutes { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public DateTime AppointmentDate { get; set; }
        
        [MaxLength(10)]
        public string AppointmentTime { get; set; }
        
        [MaxLength(500)]
        public string SpecialRequests { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Pending, Confirmed, Completed, Cancelled
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? ConfirmedAt { get; set; }
        
        public DateTime? CompletedAt { get; set; }
    }
}