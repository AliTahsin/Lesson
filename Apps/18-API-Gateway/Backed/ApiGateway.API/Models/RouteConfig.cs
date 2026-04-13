namespace ApiGateway.API.Models
{
    public class RouteConfig
    {
        public string Path { get; set; }
        public string Destination { get; set; }
        public List<string> AllowedRoles { get; set; }
        public bool RequireAuthentication { get; set; }
        public int RateLimit { get; set; }
        public string RateLimitPeriod { get; set; }
    }
}