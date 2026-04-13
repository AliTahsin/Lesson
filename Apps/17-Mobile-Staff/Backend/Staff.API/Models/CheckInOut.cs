using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staff.API.Models
{
    public class CheckInOut
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ReservationId { get; set; }
        
        [Required]
        public int GuestId { get; set; }
        
        [MaxLength(100)]
        public string GuestName { get; set; }
        
        [Required]
        public int RoomId { get; set; }
        
        [MaxLength(50)]
        public string RoomNumber { get; set; }
        
        public int? ProcessedByStaffId { get; set; }
        
        [MaxLength(100)]
        public string ProcessedByStaffName { get; set; }
        
        [MaxLength(50)]
        public string Type { get; set; } // CheckIn, CheckOut
        
        public DateTime ProcessedAt { get; set; }
        
        [MaxLength(500)]
        public string Notes { get; set; }
        
        public string DigitalKey { get; set; }
    }
}