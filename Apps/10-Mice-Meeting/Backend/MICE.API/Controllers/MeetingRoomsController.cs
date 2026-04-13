using Microsoft.AspNetCore.Mvc;
using MICE.API.DTOs;
using MICE.API.Services;

namespace MICE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingRoomsController : ControllerBase
    {
        private readonly IMeetingRoomService _roomService;

        public MeetingRoomsController(IMeetingRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var rooms = await _roomService.GetRoomsByHotelAsync(hotelId);
            return Ok(rooms);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int capacity)
        {
            var rooms = await _roomService.GetAvailableRoomsAsync(startDate, endDate, capacity);
            return Ok(rooms);
        }

        [HttpGet("{id}/availability")]
        public async Task<IActionResult> CheckAvailability(
            int id,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var isAvailable = await _roomService.CheckAvailabilityAsync(id, startDate, endDate);
            return Ok(new { roomId = id, startDate, endDate, isAvailable });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMeetingRoomDto dto)
        {
            var room = await _roomService.CreateRoomAsync(dto);
            return Ok(room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMeetingRoomDto dto)
        {
            try
            {
                var room = await _roomService.UpdateRoomAsync(id, dto);
                return Ok(room);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roomService.DeleteRoomAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Meeting room deleted successfully" });
        }
    }
}