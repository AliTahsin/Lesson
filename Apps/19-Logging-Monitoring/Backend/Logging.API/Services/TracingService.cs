using AutoMapper;
using Logging.API.Models;
using Logging.API.DTOs;
using Logging.API.Data;

namespace Logging.API.Services
{
    public class TracingService : ITracingService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TracingService> _logger;

        public TracingService(IMapper mapper, ILogger<TracingService> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TraceDto> GetTraceByIdAsync(string traceId)
        {
            var trace = MockData.GetTraces().FirstOrDefault(t => t.TraceId == traceId);
            return trace != null ? _mapper.Map<TraceDto>(trace) : null;
        }

        public async Task<List<TraceDto>> GetTracesByServiceAsync(string service, DateTime startDate, DateTime endDate)
        {
            var traces = MockData.GetTraces()
                .Where(t => t.Service == service && t.StartTime >= startDate && t.StartTime <= endDate)
                .OrderByDescending(t => t.StartTime)
                .Take(100)
                .ToList();
            
            return _mapper.Map<List<TraceDto>>(traces);
        }

        public async Task<List<TraceDto>> GetSlowTracesAsync(long minDurationMs, DateTime startDate, DateTime endDate)
        {
            var traces = MockData.GetTraces()
                .Where(t => t.DurationMs >= minDurationMs && t.StartTime >= startDate && t.StartTime <= endDate)
                .OrderByDescending(t => t.DurationMs)
                .Take(50)
                .ToList();
            
            return _mapper.Map<List<TraceDto>>(traces);
        }

        public async Task<TraceStatisticsDto> GetTraceStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            var traces = MockData.GetTraces()
                .Where(t => t.StartTime >= startDate && t.StartTime <= endDate)
                .ToList();
            
            return new TraceStatisticsDto
            {
                TotalTraces = traces.Count,
                AverageDurationMs = traces.Any() ? traces.Average(t => t.DurationMs) : 0,
                MaxDurationMs = traces.Any() ? traces.Max(t => t.DurationMs) : 0,
                MinDurationMs = traces.Any() ? traces.Min(t => t.DurationMs) : 0,
                SlowTracesCount = traces.Count(t => t.DurationMs > 1000),
                StartDate = startDate,
                EndDate = endDate
            };
        }
    }
}