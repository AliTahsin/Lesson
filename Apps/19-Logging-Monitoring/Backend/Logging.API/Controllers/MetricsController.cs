using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logging.API.Services;

namespace Logging.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly IMetricsService _metricsService;

        public MetricsController(IMetricsService metricsService)
        {
            _metricsService = metricsService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var metrics = await _metricsService.GetMetricsSummaryAsync();
            return Ok(metrics);
        }

        [HttpGet("service/{service}")]
        public async Task<IActionResult> GetByService(string service)
        {
            var metrics = await _metricsService.GetServiceMetricsAsync(service);
            return Ok(metrics);
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetAllServicesMetrics()
        {
            var services = new[] { "HotelManagement", "RoomManagement", "ReservationSystem", "DynamicPricing", "AuthRBAC", "PaymentInvoice" };
            var result = new List<object>();
            
            foreach (var service in services)
            {
                var metrics = await _metricsService.GetServiceMetricsAsync(service);
                result.Add(metrics);
            }
            
            return Ok(result);
        }

        [HttpGet("endpoints")]
        public async Task<IActionResult> GetEndpointMetrics()
        {
            var metrics = await _metricsService.GetEndpointMetricsAsync();
            return Ok(metrics);
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetRequestMetrics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var metrics = await _metricsService.GetRequestMetricsAsync(startDate, endDate);
            return Ok(metrics);
        }
    }
}