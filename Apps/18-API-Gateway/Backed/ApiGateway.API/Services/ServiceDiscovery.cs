using System.Net;
using System.Text.Json;
using ApiGateway.API.Models;

namespace ApiGateway.API.Services
{
    public class ServiceDiscovery : IServiceDiscovery
    {
        private readonly List<Service> _services;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ServiceDiscovery> _logger;

        public ServiceDiscovery(
            IHttpClientFactory httpClientFactory,
            ILogger<ServiceDiscovery> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _services = new List<Service>();
        }

        public async Task<List<Service>> GetAllServicesAsync()
        {
            return await Task.FromResult(_services.ToList());
        }

        public async Task<Service> GetServiceAsync(string serviceName)
        {
            return await Task.FromResult(_services.FirstOrDefault(s => s.Name == serviceName));
        }

        public async Task<bool> RegisterServiceAsync(Service service)
        {
            try
            {
                var existing = _services.FirstOrDefault(s => s.Name == service.Name);
                if (existing != null)
                {
                    _services.Remove(existing);
                }
                
                service.RegisteredAt = DateTime.Now;
                service.IsHealthy = true;
                service.LastHeartbeat = DateTime.Now;
                _services.Add(service);
                
                _logger.LogInformation("Service {ServiceName} registered at {Url}", service.Name, service.Url);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register service {ServiceName}", service.Name);
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> DeregisterServiceAsync(string serviceName)
        {
            var service = _services.FirstOrDefault(s => s.Name == serviceName);
            if (service != null)
            {
                _services.Remove(service);
                _logger.LogInformation("Service {ServiceName} deregistered", serviceName);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> HeartbeatAsync(string serviceName)
        {
            var service = _services.FirstOrDefault(s => s.Name == serviceName);
            if (service != null)
            {
                service.LastHeartbeat = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<ServiceHealthCheck> CheckHealthAsync(string serviceName)
        {
            var service = _services.FirstOrDefault(s => s.Name == serviceName);
            if (service == null)
            {
                return new ServiceHealthCheck
                {
                    ServiceName = serviceName,
                    IsHealthy = false,
                    CheckedAt = DateTime.Now,
                    ErrorMessage = "Service not found"
                };
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                
                var response = await client.GetAsync($"{service.Url}/health");
                stopwatch.Stop();
                
                var isHealthy = response.StatusCode == HttpStatusCode.OK;
                service.IsHealthy = isHealthy;
                
                return new ServiceHealthCheck
                {
                    ServiceName = serviceName,
                    IsHealthy = isHealthy,
                    ResponseTime = (int)stopwatch.ElapsedMilliseconds,
                    CheckedAt = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                service.IsHealthy = false;
                return new ServiceHealthCheck
                {
                    ServiceName = serviceName,
                    IsHealthy = false,
                    CheckedAt = DateTime.Now,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<List<ServiceHealthCheck>> CheckAllHealthAsync()
        {
            var results = new List<ServiceHealthCheck>();
            foreach (var service in _services)
            {
                var result = await CheckHealthAsync(service.Name);
                results.Add(result);
            }
            return await Task.FromResult(results);
        }
    }
}