using System.ComponentModel.DataAnnotations;

namespace Logging.API.Models
{
    public class Alert
    {
        [Key]
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Severity { get; set; } // Info, Warning, Critical
        
        public string Message { get; set; }
        
        public string Condition { get; set; }
        
        public double CurrentValue { get; set; }
        
        public double Threshold { get; set; }
        
        public string Service { get; set; }
        
        public DateTime TriggeredAt { get; set; }
        
        public DateTime? ResolvedAt { get; set; }
        
        public bool IsResolved { get; set; }
    }
}