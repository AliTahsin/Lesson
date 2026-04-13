using Microsoft.AspNetCore.Mvc;
using MICE.API.DTOs;
using MICE.API.Services;

namespace MICE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendeesController : ControllerBase
    {
        private readonly IAttendeeService _attendeeService;

        public AttendeesController(IAttendeeService attendeeService)
        {
            _attendeeService = attendeeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendee = await _attendeeService.GetAttendeeByIdAsync(id);
            if (attendee == null) return NotFound();
            return Ok(attendee);
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEvent(int eventId)
        {
            var attendees = await _attendeeService.GetAttendeesByEventAsync(eventId);
            return Ok(attendees);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateAttendeeDto dto)
        {
            var attendee = await _attendeeService.RegisterAttendeeAsync(dto);
            return Ok(attendee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAttendeeDto dto)
        {
            try
            {
                var attendee = await _attendeeService.UpdateAttendeeAsync(id, dto);
                return Ok(attendee);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost("{id}/checkin")]
        public async Task<IActionResult> CheckIn(int id)
        {
            var attendee = await _attendeeService.CheckInAsync(id);
            return Ok(attendee);
        }

        [HttpPost("checkin/qrcode")]
        public async Task<IActionResult> CheckInByQrCode([FromQuery] string qrCode)
        {
            try
            {
                var attendee = await _attendeeService.CheckInByQrCodeAsync(qrCode);
                return Ok(attendee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _attendeeService.DeleteAttendeeAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Attendee deleted successfully" });
        }

        [HttpGet("event/{eventId}/statistics")]
        public async Task<IActionResult> GetStatistics(int eventId)
        {
            var stats = await _attendeeService.GetAttendeeStatisticsAsync(eventId);
            return Ok(stats);
        }
    }
}