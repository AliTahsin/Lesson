using ApiGateway.API.Models;

namespace ApiGateway.API.Services
{
    public interface IServiceDiscovery
    {
        Task<List<Service>> GetAllServicesAsync();
        Task<Service> GetServiceAsync(string serviceName);
        Task<bool> RegisterServiceAsync(Service service);
        Task<bool> DeregisterServiceAsync(string serviceName);
        Task<bool> HeartbeatAsync(string serviceName);
        Task<ServiceHealthCheck> CheckHealthAsync(string serviceName);
        Task<List<ServiceHealthCheck>> CheckAllHealthAsync();
    }
}