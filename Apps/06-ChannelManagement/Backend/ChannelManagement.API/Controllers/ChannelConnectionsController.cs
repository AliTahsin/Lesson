using Microsoft.AspNetCore.Mvc;
using ChannelManagement.API.Services;
using ChannelManagement.API.DTOs;

namespace ChannelManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChannelConnectionsController : ControllerBase
    {
        private readonly ChannelSyncService _service;

        public ChannelConnectionsController(ChannelSyncService service)
        {
            _service = service;
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var connections = await _service.GetHotelConnectionsAsync(hotelId);
            return Ok(connections);
        }

        [HttpPost]
        public async Task<IActionResult> Connect([FromBody] ConnectChannelDto dto)
        {
            try
            {
                var connection = await _service.ConnectChannelAsync(dto);
                return Ok(new { message = "Channel connected successfully", connection });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{connectionId}")]
        public async Task<IActionResult> Disconnect(int connectionId)
        {
            if (await _service.DisconnectChannelAsync(connectionId))
                return Ok(new { message = "Channel disconnected successfully" });
            return NotFound();
        }
    }
}