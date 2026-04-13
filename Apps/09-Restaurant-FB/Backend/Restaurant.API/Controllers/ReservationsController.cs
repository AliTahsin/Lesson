using Microsoft.AspNetCore.Mvc;
using Restaurant.API.DTOs;
using Restaurant.API.Services;

namespace Restaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetByRestaurant(int restaurantId)
        {
            var reservations = await _reservationService.GetReservationsByRestaurantAsync(restaurantId);
            return Ok(reservations);
        }

        [HttpGet("restaurant/{restaurantId}/date")]
        public async Task<IActionResult> GetByDate(int restaurantId, [FromQuery] DateTime date)
        {
            var reservations = await _reservationService.GetReservationsByDateAsync(restaurantId, date);
            return Ok(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            try
            {
                var reservation = await _reservationService.CreateReservationAsync(dto);
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id)
        {
            var reservation = await _reservationService.ConfirmReservationAsync(id);
            return Ok(reservation);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id, [FromQuery] string reason)
        {
            var reservation = await _reservationService.CancelReservationAsync(id, reason ?? "Customer request");
            return Ok(reservation);
        }

        [HttpGet("available-tables")]
        public async Task<IActionResult> GetAvailableTables(
            [FromQuery] int restaurantId,
            [FromQuery] DateTime date,
            [FromQuery] string time,
            [FromQuery] int guestCount)
        {
            var tables = await _reservationService.GetAvailableTablesAsync(restaurantId, date, time, guestCount);
            return Ok(tables);
        }
    }
}