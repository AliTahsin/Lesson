using System.ComponentModel.DataAnnotations;

namespace Logging.API.Models
{
    public class Metric
    {
        [Key]
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Type { get; set; } // Counter, Gauge, Histogram
        
        public double Value { get; set; }
        
        public Dictionary<string, string> Labels { get; set; }
        
        public DateTime Timestamp { get; set; }
        
        public string Service { get; set; }
        
        public string Endpoint { get; set; }
    }
}