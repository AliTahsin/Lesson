using Microsoft.AspNetCore.Mvc;
using PaymentInvoice.API.DTOs;
using PaymentInvoice.API.Services;

namespace PaymentInvoice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefundsController : ControllerBase
    {
        private readonly IRefundService _refundService;

        public RefundsController(IRefundService refundService)
        {
            _refundService = refundService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessRefund([FromBody] RefundRequestDto request)
        {
            try
            {
                var result = await _refundService.ProcessRefundAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("payment/{paymentId}")]
        public async Task<IActionResult> GetByPayment(int paymentId)
        {
            var refunds = await _refundService.GetRefundsByPaymentAsync(paymentId);
            return Ok(refunds);
        }

        [HttpGet("reservation/{reservationId}")]
        public async Task<IActionResult> GetByReservation(int reservationId)
        {
            var refunds = await _refundService.GetRefundsByReservationAsync(reservationId);
            return Ok(refunds);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var stats = await _refundService.GetRefundStatisticsAsync(startDate, endDate);
            return Ok(stats);
        }
    }
}