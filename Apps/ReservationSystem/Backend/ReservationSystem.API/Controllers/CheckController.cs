using Microsoft.AspNetCore.Mvc;
using ReservationSystem.API.Services;
using ReservationSystem.API.DTOs;

namespace ReservationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckController : ControllerBase
    {
        private readonly ReservationService _service;

        public CheckController()
        {
            _service = new ReservationService();
        }

        [HttpPost("checkin")]
        public IActionResult CheckIn([FromBody] CheckInDto dto)
        {
            try
            {
                var result = _service.CheckIn(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("checkout")]
        public IActionResult CheckOut([FromBody] CheckOutDto dto)
        {
            try
            {
                var result = _service.CheckOut(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("availability")]
        public IActionResult CheckAvailability(
            [FromQuery] int roomId,
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut)
        {
            var isAvailable = _service.IsRoomAvailable(roomId, checkIn, checkOut);
            return Ok(new { roomId, checkIn, checkOut, isAvailable });
        }
    }
}