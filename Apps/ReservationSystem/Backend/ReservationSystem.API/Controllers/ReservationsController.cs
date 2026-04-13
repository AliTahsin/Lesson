using Microsoft.AspNetCore.Mvc;
using ReservationSystem.API.Services;
using ReservationSystem.API.DTOs;

namespace ReservationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationService _service;

        public ReservationsController()
        {
            _service = new ReservationService();
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAllReservations());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var reservation = _service.GetReservationById(id);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        [HttpGet("number/{reservationNumber}")]
        public IActionResult GetByNumber(string reservationNumber)
        {
            var reservation = _service.GetReservationByNumber(reservationNumber);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        [HttpGet("guest/{email}")]
        public IActionResult GetByGuestEmail(string email)
        {
            return Ok(_service.GetReservationsByGuestEmail(email));
        }

        [HttpGet("hotel/{hotelId}")]
        public IActionResult GetByHotel(int hotelId)
        {
            return Ok(_service.GetReservationsByHotel(hotelId));
        }

        [HttpGet("arrivals/today")]
        public IActionResult GetTodayArrivals() => Ok(_service.GetTodayArrivals());

        [HttpGet("departures/today")]
        public IActionResult GetTodayDepartures() => Ok(_service.GetTodayDepartures());

        [HttpGet("stats")]
        public IActionResult GetStats([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            return Ok(_service.GetStatistics(startDate, endDate));
        }

        [HttpGet("history/{reservationId}")]
        public IActionResult GetHistory(int reservationId)
        {
            return Ok(_service.GetReservationHistory(reservationId));
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateReservationDto dto)
        {
            try
            {
                var result = _service.CreateReservation(dto);
                return Ok(new { message = "Reservation created successfully", reservation = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateReservationDto dto)
        {
            try
            {
                var result = _service.UpdateReservation(id, dto);
                return Ok(new { message = "Reservation updated successfully", reservation = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Cancel(int id, [FromQuery] string reason)
        {
            try
            {
                _service.CancelReservation(id, reason ?? "User requested cancellation");
                return Ok(new { message = "Reservation cancelled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}