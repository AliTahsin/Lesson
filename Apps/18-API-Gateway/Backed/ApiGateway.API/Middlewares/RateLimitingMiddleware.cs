using System.Collections.Concurrent;
using System.Net;

namespace ApiGateway.API.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private readonly IConfiguration _configuration;
        private static readonly ConcurrentDictionary<string, ClientRequestInfo> _clientRequests = new();

        public RateLimitingMiddleware(
            RequestDelegate next,
            ILogger<RateLimitingMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var enabled = _configuration.GetValue<bool>("RateLimiting:Enabled", true);
            if (!enabled)
            {
                await _next(context);
                return;
            }

            var clientIp = GetClientIp(context);
            
            // Check whitelist
            if (IsIpWhitelisted(clientIp))
            {
                await _next(context);
                return;
            }

            var rateLimit = _configuration.GetValue<int>("RateLimiting:GeneralRateLimit", 100);
            var rateLimitPeriod = _configuration.GetValue<string>("RateLimiting:GeneralRateLimitPeriod", "1m");
            var periodSeconds = ParsePeriodToSeconds(rateLimitPeriod);
            
            var now = DateTime.UtcNow;
            var clientInfo = _clientRequests.GetOrAdd(clientIp, new ClientRequestInfo());
            
            lock (clientInfo)
            {
                // Clean old requests
                clientInfo.Requests.RemoveAll(r => r < now.AddSeconds(-periodSeconds));
                
                if (clientInfo.Requests.Count >= rateLimit)
                {
                    _logger.LogWarning("Rate limit exceeded for IP {ClientIp}", clientIp);
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.Headers.RetryAfter = periodSeconds.ToString();
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new 
                    { 
                        error = "Too many requests",
                        retryAfter = periodSeconds,
                        limit = rateLimit,
                        period = rateLimitPeriod
                    });
                    return;
                }
                
                clientInfo.Requests.Add(now);
            }
            
            await _next(context);
        }

        private string GetClientIp(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress?.ToString();
            }
            return ip ?? "unknown";
        }

        private bool IsIpWhitelisted(string ip)
        {
            var whitelist = _configuration.GetSection("RateLimiting:WhitelistedIPs").Get<string[]>();
            if (whitelist == null) return false;
            
            return whitelist.Any(w => w == ip || ip.StartsWith(w.Replace("/24", "")));
        }

        private int ParsePeriodToSeconds(string period)
        {
            return period.ToLower() switch
            {
                "1s" => 1,
                "5s" => 5,
                "10s" => 10,
                "30s" => 30,
                "1m" => 60,
                "5m" => 300,
                "10m" => 600,
                "30m" => 1800,
                "1h" => 3600,
                _ => 60
            };
        }

        private class ClientRequestInfo
        {
            public List<DateTime> Requests { get; } = new();
        }
    }
}