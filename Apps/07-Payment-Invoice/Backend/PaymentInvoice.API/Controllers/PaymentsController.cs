using Microsoft.AspNetCore.Mvc;
using PaymentInvoice.API.DTOs;
using PaymentInvoice.API.Services;

namespace PaymentInvoice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto request)
        {
            try
            {
                var result = await _paymentService.ProcessPaymentAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        [HttpGet("reservation/{reservationId}")]
        public async Task<IActionResult> GetPaymentsByReservation(int reservationId)
        {
            var payments = await _paymentService.GetPaymentsByReservationAsync(reservationId);
            return Ok(payments);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetPaymentsByCustomer(int customerId)
        {
            var payments = await _paymentService.GetPaymentsByCustomerAsync(customerId);
            return Ok(payments);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var stats = await _paymentService.GetPaymentStatisticsAsync(startDate, endDate);
            return Ok(stats);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelPayment(int id, [FromQuery] string reason)
        {
            try
            {
                var result = await _paymentService.CancelPaymentAsync(id, reason ?? "User requested");
                return Ok(new { success = result, message = "Payment cancelled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}