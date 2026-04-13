using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Housekeeping.API.Models
{
    public class HousekeepingTask
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TaskNumber { get; set; }
        
        [Required]
        public int HotelId { get; set; }
        
        [Required]
        public int RoomId { get; set; }
        
        [MaxLength(50)]
        public string RoomNumber { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TaskType { get; set; } // CheckOut, StayOver, DeepClean, Inspection
        
        [MaxLength(50)]
        public string Priority { get; set; } // High, Medium, Low
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        public int? AssignedToStaffId { get; set; }
        
        [MaxLength(100)]
        public string AssignedToStaffName { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? ScheduledDate { get; set; }
        
        public DateTime? StartedAt { get; set; }
        
        public DateTime? CompletedAt { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Pending, Assigned, InProgress, Completed, Cancelled
        
        [MaxLength(500)]
        public string Notes { get; set; }
        
        public List<string> BeforeImages { get; set; }
        
        public List<string> AfterImages { get; set; }
        
        public int EstimatedMinutes { get; set; }
        
        public int ActualMinutes { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal StaffBonus { get; set; }
    }
}