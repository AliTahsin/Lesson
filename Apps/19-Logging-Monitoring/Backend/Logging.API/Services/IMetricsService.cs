using Logging.API.DTOs;

namespace Logging.API.Services
{
    public interface IMetricsService
    {
        Task<MetricsSummaryDto> GetMetricsSummaryAsync();
        Task<List<MetricDto>> GetMetricsByServiceAsync(string service);
        Task<ServiceMetricsDto> GetServiceMetricsAsync(string service);
        Task<List<MetricDto>> GetRequestMetricsAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<string, double>> GetEndpointMetricsAsync();
    }
}