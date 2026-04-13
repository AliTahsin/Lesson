using System.ComponentModel.DataAnnotations;

namespace Reporting.API.Models
{
    public class ScheduledReport
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        public int ReportId { get; set; }
        
        [MaxLength(50)]
        public string Frequency { get; set; } // Daily, Weekly, Monthly, Quarterly
        
        [MaxLength(20)]
        public string DayOfWeek { get; set; } // Monday, Tuesday, etc. (for weekly)
        
        public int DayOfMonth { get; set; } // 1-31 (for monthly)
        
        [MaxLength(10)]
        public string TimeOfDay { get; set; } // 09:00
        
        [MaxLength(500)]
        public string RecipientEmails { get; set; } // comma separated
        
        [MaxLength(50)]
        public string Format { get; set; } // PDF, Excel
        
        public bool IsActive { get; set; }
        
        public DateTime LastRunAt { get; set; }
        
        public DateTime NextRunAt { get; set; }
        
        [MaxLength(50)]
        public string LastRunStatus { get; set; }
        
        [MaxLength(500)]
        public string LastRunError { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}