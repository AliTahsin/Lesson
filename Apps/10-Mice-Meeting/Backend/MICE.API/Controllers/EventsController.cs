using Microsoft.AspNetCore.Mvc;
using MICE.API.DTOs;
using MICE.API.Services;

namespace MICE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null) return NotFound();
            return Ok(eventItem);
        }

        [HttpGet("number/{eventNumber}")]
        public async Task<IActionResult> GetByNumber(string eventNumber)
        {
            var eventItem = await _eventService.GetEventByNumberAsync(eventNumber);
            if (eventItem == null) return NotFound();
            return Ok(eventItem);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var events = await _eventService.GetEventsByHotelAsync(hotelId);
            return Ok(events);
        }

        [HttpGet("hotel/{hotelId}/upcoming")]
        public async Task<IActionResult> GetUpcoming(int hotelId, [FromQuery] int days = 7)
        {
            var events = await _eventService.GetUpcomingEventsAsync(hotelId, days);
            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventDto dto)
        {
            try
            {
                var eventItem = await _eventService.CreateEventAsync(dto);
                return Ok(eventItem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEventDto dto)
        {
            try
            {
                var eventItem = await _eventService.UpdateEventAsync(id, dto);
                return Ok(eventItem);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var eventItem = await _eventService.UpdateEventStatusAsync(id, status);
            return Ok(eventItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _eventService.DeleteEventAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Event deleted successfully" });
        }

        [HttpGet("{id}/calendar")]
        public async Task<IActionResult> DownloadCalendar(int id)
        {
            var calendarData = await _eventService.GenerateCalendarFileAsync(id);
            return File(calendarData, "text/calendar", $"event_{id}.ics");
        }
    }
}