using Logging.API.DTOs;

namespace Logging.API.Services
{
    public interface ILoggingService
    {
        Task<List<LogDto>> SearchLogsAsync(LogSearchDto searchDto);
        Task<LogDto> GetLogByIdAsync(string id);
        Task<LogStatisticsDto> GetLogStatisticsAsync(DateTime startDate, DateTime endDate);
        Task<List<LogDto>> GetErrorLogsAsync(DateTime startDate, DateTime endDate);
        Task<List<LogDto>> GetLogsByServiceAsync(string service, DateTime startDate, DateTime endDate);
        Task<Dictionary<string, long>> GetLogCountByLevelAsync(DateTime startDate, DateTime endDate);
    }
}