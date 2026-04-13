using Logging.API.Models;

namespace Logging.API.Data
{
    public static class MockData
    {
        private static readonly Random _random = new();
        private static readonly string[] _levels = { "Information", "Warning", "Error", "Debug" };
        private static readonly string[] _services = { "HotelManagement", "RoomManagement", "ReservationSystem", "DynamicPricing", "AuthRBAC", "PaymentInvoice" };
        private static readonly string[] _endpoints = { "/api/hotels", "/api/rooms", "/api/reservations", "/api/auth/login", "/api/payments" };
        private static readonly string[] _messages = {
            "Request completed successfully",
            "Database query executed",
            "User authenticated",
            "Payment processed",
            "Reservation created",
            "Cache hit",
            "External API call",
            "Validation failed",
            "Connection timeout",
            "Unauthorized access attempt"
        };

        public static List<LogEntry> GetLogs()
        {
            var logs = new List<LogEntry>();
            
            for (int i = 0; i < 5000; i++)
            {
                var timestamp = DateTime.Now.AddMinutes(-_random.Next(0, 10080));
                var level = GetWeightedLevel();
                var isError = level == "Error";
                var durationMs = _random.Next(5, 5000);
                
                logs.Add(new LogEntry
                {
                    Id = Guid.NewGuid().ToString(),
                    Timestamp = timestamp,
                    Level = level,
                    Message = _messages[_random.Next(_messages.Length)] + (isError ? $" - Error Code: {_random.Next(100, 500)}" : ""),
                    Exception = isError ? $"System.Exception: {_messages[_random.Next(_messages.Length)]}" : null,
                    SourceContext = _services[_random.Next(_services.Length)],
                    RequestPath = _endpoints[_random.Next(_endpoints.Length)],
                    RequestMethod = _random.Next(0, 10) > 7 ? "POST" : "GET",
                    StatusCode = isError ? _random.Next(400, 500) : _random.Next(200, 299),
                    DurationMs = durationMs,
                    UserId = _random.Next(0, 10) > 8 ? $"user_{_random.Next(1, 100)}" : null,
                    CorrelationId = Guid.NewGuid().ToString(),
                    IpAddress = $"192.168.{_random.Next(1, 255)}.{_random.Next(1, 255)}",
                    UserAgent = _random.Next(0, 10) > 8 ? "Mobile" : "Web",
                    Properties = new Dictionary<string, object>
                    {
                        ["Environment"] = _random.Next(0, 10) > 9 ? "Production" : "Development",
                        ["Version"] = "1.0.0"
                    }
                });
            }
            
            return logs.OrderByDescending(l => l.Timestamp).ToList();
        }

        public static List<Trace> GetTraces()
        {
            var traces = new List<Trace>();
            
            for (int i = 0; i < 1000; i++)
            {
                var startTime = DateTime.Now.AddMinutes(-_random.Next(0, 10080));
                var durationMs = _random.Next(10, 5000);
                
                traces.Add(new Trace
                {
                    TraceId = Guid.NewGuid().ToString(),
                    SpanId = Guid.NewGuid().ToString(),
                    ParentSpanId = _random.Next(0, 10) > 8 ? Guid.NewGuid().ToString() : null,
                    OperationName = _endpoints[_random.Next(_endpoints.Length)],
                    StartTime = startTime,
                    DurationMs = durationMs,
                    Tags = new Dictionary<string, string>
                    {
                        ["http.method"] = _random.Next(0, 10) > 7 ? "POST" : "GET",
                        ["http.status_code"] = _random.Next(200, 500).ToString(),
                        ["service"] = _services[_random.Next(_services.Length)]
                    },
                    Logs = new Dictionary<string, string>(),
                    Service = _services[_random.Next(_services.Length)],
                    Endpoint = _endpoints[_random.Next(_endpoints.Length)]
                });
            }
            
            return traces.OrderByDescending(t => t.StartTime).ToList();
        }

        private static string GetWeightedLevel()
        {
            var rand = _random.Next(1, 101);
            if (rand <= 70) return "Information";
            if (rand <= 85) return "Warning";
            if (rand <= 95) return "Error";
            return "Debug";
        }
    }
}