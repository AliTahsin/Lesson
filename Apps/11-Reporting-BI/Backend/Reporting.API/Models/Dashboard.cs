using System.ComponentModel.DataAnnotations;

namespace Reporting.API.Models
{
    public class Dashboard
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        public int HotelId { get; set; }
        
        [MaxLength(50)]
        public string DashboardType { get; set; } // Executive, Operations, Finance, Sales
        
        public List<DashboardWidget> Widgets { get; set; }
        
        public bool IsDefault { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }

    public class DashboardWidget
    {
        [Key]
        public int Id { get; set; }
        
        public int DashboardId { get; set; }
        
        [MaxLength(100)]
        public string Title { get; set; }
        
        [MaxLength(50)]
        public string WidgetType { get; set; } // Chart, KPI, Table, Map
        
        [MaxLength(50)]
        public string ChartType { get; set; } // Line, Bar, Pie, Doughnut, Area
        
        [MaxLength(500)]
        public string DataSource { get; set; } // API endpoint or query
        
        public int Width { get; set; } // 1-12 (grid)
        
        public int Height { get; set; } // 1-12 (grid)
        
        public int PositionX { get; set; }
        
        public int PositionY { get; set; }
        
        public int RefreshInterval { get; set; } // seconds
        
        [MaxLength(500)]
        public string Configuration { get; set; } // JSON string
    }
}