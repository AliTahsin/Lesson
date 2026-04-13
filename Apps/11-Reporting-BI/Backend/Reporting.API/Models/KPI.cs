using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reporting.API.Models
{
    public class KPI
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(50)]
        public string Code { get; set; } // REVPAR, ADR, OCCUPANCY, GOPPAR
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(50)]
        public string Category { get; set; } // Revenue, Operations, Customer, Financial
        
        [MaxLength(50)]
        public string Formula { get; set; }
        
        public int HotelId { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentValue { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal PreviousValue { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TargetValue { get; set; }
        
        public decimal ChangePercent { get; set; }
        
        [MaxLength(20)]
        public string Trend { get; set; } // Up, Down, Stable
        
        [MaxLength(50)]
        public string Status { get; set; } // OnTrack, Warning, Critical
        
        public DateTime LastUpdated { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}