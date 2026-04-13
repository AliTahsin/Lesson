using Microsoft.AspNetCore.Mvc;
using ChannelManagement.API.Services;
using ChannelManagement.API.DTOs;

namespace ChannelManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly ChannelSyncService _service;

        public SyncController(ChannelSyncService service)
        {
            _service = service;
        }

        [HttpPost("availability")]
        public async Task<IActionResult> SyncAvailability([FromBody] SyncAvailabilityRequest request)
        {
            var result = await _service.SyncAvailabilityAsync(request);
            return Ok(result);
        }

        [HttpPost("prices")]
        public async Task<IActionResult> SyncPrices([FromBody] SyncPriceRequest request)
        {
            var result = await _service.SyncPricesAsync(request);
            return Ok(result);
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetBookings([FromQuery] int? channelId, [FromQuery] int? hotelId)
        {
            var bookings = await _service.GetChannelBookingsAsync(channelId, hotelId);
            return Ok(bookings);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = await _service.GetDashboardStatsAsync();
            return Ok(stats);
        }

        [HttpGet("logs/{channelId}")]
        public async Task<IActionResult> GetSyncLogs(int channelId, [FromQuery] int? hotelId)
        {
            var logs = await _service.GetSyncLogsAsync(channelId, hotelId);
            return Ok(logs);
        }
    }
}