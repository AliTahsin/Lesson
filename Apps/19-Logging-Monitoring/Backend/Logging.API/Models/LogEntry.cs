using System.ComponentModel.DataAnnotations;

namespace Logging.API.Models
{
    public class LogEntry
    {
        [Key]
        public string Id { get; set; }
        
        public DateTime Timestamp { get; set; }
        
        public string Level { get; set; }
        
        public string Message { get; set; }
        
        public string Exception { get; set; }
        
        public string SourceContext { get; set; }
        
        public string RequestPath { get; set; }
        
        public string RequestMethod { get; set; }
        
        public int? StatusCode { get; set; }
        
        public long? DurationMs { get; set; }
        
        public string UserId { get; set; }
        
        public string CorrelationId { get; set; }
        
        public string IpAddress { get; set; }
        
        public string UserAgent { get; set; }
        
        public Dictionary<string, object> Properties { get; set; }
    }
}