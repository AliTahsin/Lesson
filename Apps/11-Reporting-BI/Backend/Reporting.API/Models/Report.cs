using System.ComponentModel.DataAnnotations;

namespace Reporting.API.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(50)]
        public string ReportType { get; set; } // Revenue, Occupancy, Reservation, Customer, Channel
        
        [MaxLength(50)]
        public string Format { get; set; } // PDF, Excel, CSV
        
        public int HotelId { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public DateTime GeneratedAt { get; set; }
        
        [MaxLength(500)]
        public string FileUrl { get; set; }
        
        public long FileSize { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Pending, Completed, Failed
        
        [MaxLength(500)]
        public string Parameters { get; set; } // JSON string of parameters
        
        public int GeneratedByUserId { get; set; }
        
        [MaxLength(100)]
        public string GeneratedByUserName { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}