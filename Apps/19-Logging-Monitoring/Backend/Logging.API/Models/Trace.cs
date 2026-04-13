using System.ComponentModel.DataAnnotations;

namespace Logging.API.Models
{
    public class Trace
    {
        [Key]
        public string TraceId { get; set; }
        
        public string SpanId { get; set; }
        
        public string ParentSpanId { get; set; }
        
        public string OperationName { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public long DurationMs { get; set; }
        
        public Dictionary<string, string> Tags { get; set; }
        
        public Dictionary<string, string> Logs { get; set; }
        
        public string Service { get; set; }
        
        public string Endpoint { get; set; }
    }
}