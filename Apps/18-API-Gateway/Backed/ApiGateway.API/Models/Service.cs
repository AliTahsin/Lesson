namespace ApiGateway.API.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Port { get; set; }
        public bool IsHealthy { get; set; }
        public DateTime LastHeartbeat { get; set; }
        public DateTime RegisteredAt { get; set; }
    }

    public class ServiceHealthCheck
    {
        public string ServiceName { get; set; }
        public bool IsHealthy { get; set; }
        public int ResponseTime { get; set; }
        public DateTime CheckedAt { get; set; }
        public string ErrorMessage { get; set; }
    }
}