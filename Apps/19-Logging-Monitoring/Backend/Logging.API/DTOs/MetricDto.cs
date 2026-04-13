namespace Logging.API.DTOs
{
    public class MetricDto
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public Dictionary<string, string> Labels { get; set; }
        public DateTime Timestamp { get; set; }
        public string Service { get; set; }
    }

    public class MetricsSummaryDto
    {
        public int TotalRequests { get; set; }
        public double ErrorRate { get; set; }
        public double AverageResponseTime { get; set; }
        public double RequestsPerMinute { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ServiceMetricsDto
    {
        public string ServiceName { get; set; }
        public int TotalRequests { get; set; }
        public int ErrorCount { get; set; }
        public double AverageResponseTime { get; set; }
        public DateTime LastActivity { get; set; }
        public string Status { get; set; }
    }
}