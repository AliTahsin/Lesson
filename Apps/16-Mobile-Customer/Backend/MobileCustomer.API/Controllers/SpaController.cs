using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileCustomer.API.DTOs;
using MobileCustomer.API.Services;

namespace MobileCustomer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SpaController : ControllerBase
    {
        private readonly ISpaService _spaService;

        public SpaController(ISpaService spaService)
        {
            _spaService = spaService;
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetServices()
        {
            var services = await _spaService.GetServicesAsync();
            return Ok(services);
        }

        [HttpGet("services/{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _spaService.GetServiceByIdAsync(id);
            if (service == null) return NotFound();
            return Ok(service);
        }

        [HttpGet("available-times")]
        public async Task<IActionResult> GetAvailableTimes([FromQuery] DateTime date, [FromQuery] int serviceId)
        {
            var times = await _spaService.GetAvailableTimesAsync(date, serviceId);
            return Ok(times);
        }

        [HttpPost("appointments")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateSpaAppointmentDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var appointment = await _spaService.CreateAppointmentAsync(userId, dto);
            return Ok(appointment);
        }

        [HttpGet("appointments")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var appointments = await _spaService.GetUserAppointmentsAsync(userId);
            return Ok(appointments);
        }

        [HttpGet("appointments/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentById(int appointmentId)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var appointment = await _spaService.GetAppointmentByIdAsync(appointmentId, userId);
            if (appointment == null) return NotFound();
            return Ok(appointment);
        }

        [HttpPost("appointments/{appointmentId}/cancel")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var result = await _spaService.CancelAppointmentAsync(appointmentId, userId);
            return Ok(new { success = result });
        }
    }
}