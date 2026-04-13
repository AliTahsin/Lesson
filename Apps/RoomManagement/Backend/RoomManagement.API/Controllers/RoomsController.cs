using Microsoft.AspNetCore.Mvc;
using RoomManagement.API.Services;
using RoomManagement.API.Models;

namespace RoomManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly RoomManagementService _service;

        public RoomsController()
        {
            _service = new RoomManagementService();
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAllRooms());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var room = _service.GetRoomById(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpGet("hotel/{hotelId}")]
        public IActionResult GetByHotel(int hotelId) => Ok(_service.GetRoomsByHotel(hotelId));

        [HttpGet("type/{roomTypeId}")]
        public IActionResult GetByType(int roomTypeId) => Ok(_service.GetRoomsByType(roomTypeId));

        [HttpGet("available")]
        public IActionResult GetAvailableRooms(
            [FromQuery] int hotelId,
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut)
        {
            return Ok(_service.GetAvailableRooms(hotelId, checkIn, checkOut));
        }

        [HttpGet("search")]
        public IActionResult Search(
            [FromQuery] int? hotelId,
            [FromQuery] int? roomTypeId,
            [FromQuery] int? capacity,
            [FromQuery] string view,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            return Ok(_service.SearchRooms(hotelId, roomTypeId, capacity, view, minPrice, maxPrice));
        }

        [HttpGet("stats/{hotelId}")]
        public IActionResult GetStatistics(int hotelId) => Ok(_service.GetRoomStatistics(hotelId));

        [HttpPost]
        public IActionResult Create([FromBody] Room room)
        {
            var created = _service.AddRoom(room);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Room room)
        {
            if (_service.UpdateRoom(id, room))
                return Ok(new { message = "Room updated successfully" });
            return NotFound();
        }

        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromQuery] string status)
        {
            if (_service.UpdateRoomStatus(id, status))
                return Ok(new { message = $"Room status updated to {status}" });
            return NotFound();
        }
    }
}