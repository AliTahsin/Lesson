using Microsoft.AspNetCore.Mvc;
using ApiGateway.API.Services;
using ApiGateway.API.Models;

namespace ApiGateway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly IServiceDiscovery _serviceDiscovery;
        private readonly ILogger<GatewayController> _logger;

        public GatewayController(
            IServiceDiscovery serviceDiscovery,
            ILogger<GatewayController> logger)
        {
            _serviceDiscovery = serviceDiscovery;
            _logger = logger;
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetServices()
        {
            var services = await _serviceDiscovery.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpGet("services/health")]
        public async Task<IActionResult> GetHealth()
        {
            var health = await _serviceDiscovery.CheckAllHealthAsync();
            return Ok(health);
        }

        [HttpPost("services/register")]
        public async Task<IActionResult> RegisterService([FromBody] Service service)
        {
            var result = await _serviceDiscovery.RegisterServiceAsync(service);
            if (result)
                return Ok(new { message = "Service registered successfully" });
            return BadRequest(new { error = "Failed to register service" });
        }

        [HttpDelete("services/{serviceName}")]
        public async Task<IActionResult> DeregisterService(string serviceName)
        {
            var result = await _serviceDiscovery.DeregisterServiceAsync(serviceName);
            if (result)
                return Ok(new { message = "Service deregistered successfully" });
            return NotFound(new { error = "Service not found" });
        }

        [HttpPost("services/{serviceName}/heartbeat")]
        public async Task<IActionResult> Heartbeat(string serviceName)
        {
            var result = await _serviceDiscovery.HeartbeatAsync(serviceName);
            if (result)
                return Ok(new { message = "Heartbeat received" });
            return NotFound(new { error = "Service not found" });
        }

        [HttpGet("info")]
        public IActionResult GetInfo()
        {
            return Ok(new
            {
                Name = "API Gateway",
                Version = "1.0.0",
                Services = new[] { "HotelManagement", "RoomManagement", "ReservationSystem", "DynamicPricing", "CRMLoyalty", "ChannelManagement", "PaymentInvoice", "Housekeeping", "RestaurantFB", "MICEMeeting", "ReportingBI", "AuthRBAC", "NotificationPush", "AIRecommendation", "ChatbotNLP", "MobileCustomer", "MobileStaff" },
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("routes")]
        public IActionResult GetRoutes()
        {
            var routes = new[]
            {
                new { Path = "/api/hotels/**", Service = "HotelManagement" },
                new { Path = "/api/rooms/**", Service = "RoomManagement" },
                new { Path = "/api/reservations/**", Service = "ReservationSystem" },
                new { Path = "/api/pricing/**", Service = "DynamicPricing" },
                new { Path = "/api/customers/**", Service = "CRMLoyalty" },
                new { Path = "/api/channels/**", Service = "ChannelManagement" },
                new { Path = "/api/payments/**", Service = "PaymentInvoice" },
                new { Path = "/api/tasks/**", Service = "Housekeeping" },
                new { Path = "/api/restaurants/**", Service = "RestaurantFB" },
                new { Path = "/api/meetingrooms/**", Service = "MICEMeeting" },
                new { Path = "/api/reports/**", Service = "ReportingBI" },
                new { Path = "/api/auth/**", Service = "AuthRBAC" },
                new { Path = "/api/notifications/**", Service = "NotificationPush" },
                new { Path = "/api/recommendations/**", Service = "AIRecommendation" },
                new { Path = "/api/chat/**", Service = "ChatbotNLP" },
                new { Path = "/api/customer/**", Service = "MobileCustomer" },
                new { Path = "/api/staff/**", Service = "MobileStaff" },
                new { Path = "/hubs/**", Service = "SignalR" }
            };
            return Ok(routes);
        }
    }
}