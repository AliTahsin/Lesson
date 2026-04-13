using Microsoft.Extensions.Diagnostics.HealthChecks;
using ApiGateway.API.Services;

namespace ApiGateway.API.HealthChecks
{
    public class ServiceHealthCheck : IHealthCheck
    {
        private readonly IServiceDiscovery _serviceDiscovery;
        private readonly ILogger<ServiceHealthCheck> _logger;

        public ServiceHealthCheck(
            IServiceDiscovery serviceDiscovery,
            ILogger<ServiceHealthCheck> logger)
        {
            _serviceDiscovery = serviceDiscovery;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var services = await _serviceDiscovery.GetAllServicesAsync();
                var healthChecks = await _serviceDiscovery.CheckAllHealthAsync();
                
                var unhealthyServices = healthChecks.Where(h => !h.IsHealthy).ToList();
                
                if (unhealthyServices.Any())
                {
                    return HealthCheckResult.Degraded(
                        $"Some services are unhealthy: {string.Join(", ", unhealthyServices.Select(s => s.ServiceName))}",
                        data: new Dictionary<string, object>
                        {
                            ["TotalServices"] = services.Count,
                            ["HealthyServices"] = services.Count(s => s.IsHealthy),
                            ["UnhealthyServices"] = unhealthyServices.Count,
                            ["Details"] = unhealthyServices
                        });
                }
                
                return HealthCheckResult.Healthy(
                    $"All {services.Count} services are healthy",
                    data: new Dictionary<string, object>
                    {
                        ["TotalServices"] = services.Count,
                        ["HealthyServices"] = services.Count(s => s.IsHealthy),
                        ["Timestamp"] = DateTime.UtcNow
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return HealthCheckResult.Unhealthy("Health check failed", ex);
            }
        }
    }
}