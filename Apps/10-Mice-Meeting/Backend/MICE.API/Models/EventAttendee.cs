using System.ComponentModel.DataAnnotations;

namespace MICE.API.Models
{
    public class EventAttendee
    {
        [Key]
        public int Id { get; set; }
        
        public int EventId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        
        [MaxLength(200)]
        public string Email { get; set; }
        
        [MaxLength(20)]
        public string Phone { get; set; }
        
        [MaxLength(200)]
        public string Company { get; set; }
        
        [MaxLength(100)]
        public string Title { get; set; }
        
        public DateTime? CheckInTime { get; set; }
        
        public bool HasCheckedIn { get; set; }
        
        [MaxLength(50)]
        public string DietaryRestrictions { get; set; }
        
        [MaxLength(500)]
        public string SpecialNeeds { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Registered, Confirmed, CheckedIn, Cancelled, NoShow
        
        public string QrCode { get; set; }
        
        public DateTime RegisteredAt { get; set; }
        
        public DateTime? ConfirmedAt { get; set; }
    }
}