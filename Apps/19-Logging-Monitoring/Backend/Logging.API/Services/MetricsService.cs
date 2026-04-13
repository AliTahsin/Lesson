using AutoMapper;
using Logging.API.Models;
using Logging.API.DTOs;
using Logging.API.Data;

namespace Logging.API.Services
{
    public class MetricsService : IMetricsService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<MetricsService> _logger;

        public MetricsService(IMapper mapper, ILogger<MetricsService> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MetricsSummaryDto> GetMetricsSummaryAsync()
        {
            var logs = MockData.GetLogs();
            var lastHour = DateTime.Now.AddHours(-1);
            
            return new MetricsSummaryDto
            {
                TotalRequests = logs.Count,
                ErrorRate = (double)logs.Count(l => l.Level == "Error") / logs.Count * 100,
                AverageResponseTime = logs.Where(l => l.DurationMs.HasValue).Average(l => l.DurationMs ?? 0),
                RequestsPerMinute = logs.Count(l => l.Timestamp >= lastHour) / 60.0,
                Timestamp = DateTime.Now
            };
        }

        public async Task<List<MetricDto>> GetMetricsByServiceAsync(string service)
        {
            var logs = MockData.GetLogs().Where(l => l.SourceContext != null && l.SourceContext.Contains(service)).ToList();
            return _mapper.Map<List<MetricDto>>(logs.Take(100));
        }

        public async Task<ServiceMetricsDto> GetServiceMetricsAsync(string service)
        {
            var logs = MockData.GetLogs().Where(l => l.SourceContext != null && l.SourceContext.Contains(service)).ToList();
            
            return new ServiceMetricsDto
            {
                ServiceName = service,
                TotalRequests = logs.Count,
                ErrorCount = logs.Count(l => l.Level == "Error"),
                AverageResponseTime = logs.Where(l => l.DurationMs.HasValue).Average(l => l.DurationMs ?? 0),
                LastActivity = logs.Any() ? logs.Max(l => l.Timestamp) : DateTime.Now,
                Status = logs.Any(l => l.Level == "Error" && l.Timestamp > DateTime.Now.AddMinutes(-5)) ? "Degraded" : "Healthy"
            };
        }

        public async Task<List<MetricDto>> GetRequestMetricsAsync(DateTime startDate, DateTime endDate)
        {
            var logs = MockData.GetLogs()
                .Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate)
                .ToList();
            
            return _mapper.Map<List<MetricDto>>(logs);
        }

        public async Task<Dictionary<string, double>> GetEndpointMetricsAsync()
        {
            var logs = MockData.GetLogs();
            
            return await Task.FromResult(logs
                .Where(l => !string.IsNullOrEmpty(l.RequestPath))
                .GroupBy(l => l.RequestPath)
                .ToDictionary(g => g.Key, g => g.Average(l => l.DurationMs ?? 0)));
        }
    }
}