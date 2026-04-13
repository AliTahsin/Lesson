using System.Text;
using System.Text.Json;

namespace ApiGateway.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            var requestId = Guid.NewGuid().ToString();
            
            // Log request
            var requestLog = new
            {
                RequestId = requestId,
                Method = context.Request.Method,
                Path = context.Request.Path,
                QueryString = context.Request.QueryString.ToString(),
                ClientIp = GetClientIp(context),
                UserAgent = context.Request.Headers["User-Agent"].ToString(),
                Timestamp = startTime
            };
            
            _logger.LogInformation("Request: {RequestLog}", JsonSerializer.Serialize(requestLog));
            
            // Capture response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            
            await _next(context);
            
            var endTime = DateTime.UtcNow;
            var duration = endTime - startTime;
            
            // Log response
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);
            
            var responseLog = new
            {
                RequestId = requestId,
                StatusCode = context.Response.StatusCode,
                DurationMs = duration.TotalMilliseconds,
                ResponseSize = responseBody.Length,
                Timestamp = endTime
            };
            
            _logger.LogInformation("Response: {ResponseLog}", JsonSerializer.Serialize(responseLog));
            
            // Log slow requests
            if (duration.TotalMilliseconds > 1000)
            {
                _logger.LogWarning("Slow request: {Path} took {DurationMs}ms", context.Request.Path, duration.TotalMilliseconds);
            }
            
            await responseBody.CopyToAsync(originalBodyStream);
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
    }
}