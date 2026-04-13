namespace Logging.API.DTOs
{
    public class LogDto
    {
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
    }

    public class LogSearchDto
    {
        public string Level { get; set; }
        public string Service { get; set; }
        public string CorrelationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SearchText { get; set; }
    }

    public class LogStatisticsDto
    {
        public int TotalLogs { get; set; }
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
        public int InfoCount { get; set; }
        public int DebugCount { get; set; }
        public double AverageResponseTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}