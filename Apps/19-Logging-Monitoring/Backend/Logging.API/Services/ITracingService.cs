using Logging.API.DTOs;

namespace Logging.API.Services
{
    public interface ITracingService
    {
        Task<TraceDto> GetTraceByIdAsync(string traceId);
        Task<List<TraceDto>> GetTracesByServiceAsync(string service, DateTime startDate, DateTime endDate);
        Task<List<TraceDto>> GetSlowTracesAsync(long minDurationMs, DateTime startDate, DateTime endDate);
        Task<TraceStatisticsDto> GetTraceStatisticsAsync(DateTime startDate, DateTime endDate);
    }
}