using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staff.API.Models
{
    public class MaintenanceIssue
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string IssueNumber { get; set; }
        
        [Required]
        public int HotelId { get; set; }
        
        [Required]
        public int RoomId { get; set; }
        
        [MaxLength(50)]
        public string RoomNumber { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Category { get; set; } // Plumbing, Electrical, HVAC, Furniture, Appliance
        
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(50)]
        public string Priority { get; set; } // Critical, High, Medium, Low
        
        public int? ReportedByStaffId { get; set; }
        
        [MaxLength(100)]
        public string ReportedByName { get; set; }
        
        public DateTime ReportedAt { get; set; }
        
        public int? AssignedToStaffId { get; set; }
        
        [MaxLength(100)]
        public string AssignedToStaffName { get; set; }
        
        public DateTime? AssignedAt { get; set; }
        
        public DateTime? StartedAt { get; set; }
        
        public DateTime? ResolvedAt { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Reported, Assigned, InProgress, Resolved, Closed
        
        [MaxLength(500)]
        public string ResolutionNotes { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedCost { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal ActualCost { get; set; }
        
        public List<string> Images { get; set; }
    }
}