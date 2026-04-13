using Microsoft.AspNetCore.Mvc;
using ChannelManagement.API.Services;
using ChannelManagement.API.DTOs;

namespace ChannelManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChannelsController : ControllerBase
    {
        private readonly ChannelSyncService _service;

        public ChannelsController(ChannelSyncService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var channels = await _service.GetAllChannelsAsync();
            return Ok(channels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var channel = await _service.GetChannelByIdAsync(id);
            if (channel == null) return NotFound();
            return Ok(channel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChannelDto dto)
        {
            var channel = await _service.CreateChannelAsync(dto);
            return Ok(new { message = "Channel created successfully", channel });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ChannelDetailDto dto)
        {
            if (await _service.UpdateChannelAsync(id, dto))
                return Ok(new { message = "Channel updated successfully" });
            return NotFound();
        }
    }
}