using AutoMapper;
using Logging.API.Models;
using Logging.API.DTOs;
using Logging.API.Data;
using Serilog;

namespace Logging.API.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(IMapper mapper, ILogger<LoggingService> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<LogDto>> SearchLogsAsync(LogSearchDto searchDto)
        {
            var logs = MockData.GetLogs();
            
            var query = logs.AsQueryable();
            
            if (!string.IsNullOrEmpty(searchDto.Level))
                query = query.Where(l => l.Level == searchDto.Level);
            
            if (!string.IsNullOrEmpty(searchDto.Service))
                query = query.Where(l => l.SourceContext != null && l.SourceContext.Contains(searchDto.Service));
            
            if (!string.IsNullOrEmpty(searchDto.CorrelationId))
                query = query.Where(l => l.CorrelationId == searchDto.CorrelationId);
            
            if (searchDto.StartDate.HasValue)
                query = query.Where(l => l.Timestamp >= searchDto.StartDate.Value);
            
            if (searchDto.EndDate.HasValue)
                query = query.Where(l => l.Timestamp <= searchDto.EndDate.Value);
            
            if (!string.IsNullOrEmpty(searchDto.SearchText))
                query = query.Where(l => l.Message.Contains(searchDto.SearchText));
            
            var result = query.OrderByDescending(l => l.Timestamp).Take(100).ToList();
            return _mapper.Map<List<LogDto>>(result);
        }

        public async Task<LogDto> GetLogByIdAsync(string id)
        {
            var log = MockData.GetLogs().FirstOrDefault(l => l.Id == id);
            return log != null ? _mapper.Map<LogDto>(log) : null;
        }

        public async Task<LogStatisticsDto> GetLogStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            var logs = MockData.GetLogs().Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate).ToList();
            
            return new LogStatisticsDto
            {
                TotalLogs = logs.Count,
                ErrorCount = logs.Count(l => l.Level == "Error"),
                WarningCount = logs.Count(l => l.Level == "Warning"),
                InfoCount = logs.Count(l => l.Level == "Information"),
                DebugCount = logs.Count(l => l.Level == "Debug"),
                AverageResponseTime = logs.Where(l => l.DurationMs.HasValue).Average(l => l.DurationMs ?? 0),
                StartDate = startDate,
                EndDate = endDate
            };
        }

        public async Task<List<LogDto>> GetErrorLogsAsync(DateTime startDate, DateTime endDate)
        {
            var logs = MockData.GetLogs()
                .Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate && l.Level == "Error")
                .OrderByDescending(l => l.Timestamp)
                .Take(100)
                .ToList();
            
            return _mapper.Map<List<LogDto>>(logs);
        }

        public async Task<List<LogDto>> GetLogsByServiceAsync(string service, DateTime startDate, DateTime endDate)
        {
            var logs = MockData.GetLogs()
                .Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate && 
                       l.SourceContext != null && l.SourceContext.Contains(service))
                .OrderByDescending(l => l.Timestamp)
                .Take(100)
                .ToList();
            
            return _mapper.Map<List<LogDto>>(logs);
        }

        public async Task<Dictionary<string, long>> GetLogCountByLevelAsync(DateTime startDate, DateTime endDate)
        {
            var logs = MockData.GetLogs().Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate).ToList();
            
            return await Task.FromResult(new Dictionary<string, long>
            {
                ["Information"] = logs.Count(l => l.Level == "Information"),
                ["Warning"] = logs.Count(l => l.Level == "Warning"),
                ["Error"] = logs.Count(l => l.Level == "Error"),
                ["Debug"] = logs.Count(l => l.Level == "Debug")
            });
        }
    }
}