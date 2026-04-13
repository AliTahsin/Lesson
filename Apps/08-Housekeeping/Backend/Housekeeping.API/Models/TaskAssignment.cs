using System.ComponentModel.DataAnnotations;

namespace Housekeeping.API.Models
{
    public class TaskAssignment
    {
        [Key]
        public int Id { get; set; }
        
        public int TaskId { get; set; }
        
        public int StaffId { get; set; }
        
        [MaxLength(100)]
        public string StaffName { get; set; }
        
        public DateTime AssignedAt { get; set; }
        
        public DateTime? AcceptedAt { get; set; }
        
        public DateTime? RejectedAt { get; set; }
        
        [MaxLength(500)]
        public string RejectionReason { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Pending, Accepted, Rejected, Completed
    }
}