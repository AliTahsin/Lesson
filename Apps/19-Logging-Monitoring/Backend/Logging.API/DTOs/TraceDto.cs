namespace Logging.API.DTOs
{
    public class TraceDto
    {
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }
        public string OperationName { get; set; }
        public DateTime StartTime { get; set; }
        public long DurationMs { get; set; }
        public Dictionary<string, string> Tags { get; set; }
        public string Service { get; set; }
        public string Endpoint { get; set; }
    }

    public class TraceStatisticsDto
    {
        public int TotalTraces { get; set; }
        public double AverageDurationMs { get; set; }
        public long MaxDurationMs { get; set; }
        public long MinDurationMs { get; set; }
        public int SlowTracesCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}