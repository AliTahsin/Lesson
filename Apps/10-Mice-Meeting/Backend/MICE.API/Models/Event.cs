using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MICE.API.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string EventNumber { get; set; }
        
        [Required]
        public int HotelId { get; set; }
        
        [Required]
        public int MeetingRoomId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [MaxLength(1000)]
        public string Description { get; set; }
        
        [MaxLength(50)]
        public string EventType { get; set; } // Conference, Seminar, Wedding, Gala, Training
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public DateTime SetupStartTime { get; set; }
        
        public DateTime SetupEndTime { get; set; }
        
        public int ExpectedAttendees { get; set; }
        
        public int ActualAttendees { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBudget { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal ActualCost { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Planned, Confirmed, InProgress, Completed, Cancelled
        
        public int? CustomerId { get; set; }
        
        [MaxLength(200)]
        public string CustomerName { get; set; }
        
        [MaxLength(200)]
        public string CustomerEmail { get; set; }
        
        [MaxLength(20)]
        public string CustomerPhone { get; set; }
        
        [MaxLength(200)]
        public string CustomerCompany { get; set; }
        
        [MaxLength(500)]
        public string SpecialRequests { get; set; }
        
        public List<EventSchedule> Schedule { get; set; }
        
        public List<int> EquipmentIds { get; set; }
        
        public List<string> Images { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? ConfirmedAt { get; set; }
        
        public DateTime? CompletedAt { get; set; }
    }

    public class EventSchedule
    {
        [Key]
        public int Id { get; set; }
        
        public int EventId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        [MaxLength(100)]
        public string Speaker { get; set; }
        
        [MaxLength(200)]
        public string Location { get; set; }
        
        public int Order { get; set; }
    }
}