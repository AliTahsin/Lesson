using Microsoft.AspNetCore.Mvc;
using PaymentInvoice.API.DTOs;
using PaymentInvoice.API.Services;

namespace PaymentInvoice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();
            return Ok(invoice);
        }

        [HttpGet("reservation/{reservationId}")]
        public async Task<IActionResult> GetByReservation(int reservationId)
        {
            var invoice = await _invoiceService.GetInvoiceByReservationAsync(reservationId);
            if (invoice == null)
                return NotFound();
            return Ok(invoice);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var invoices = await _invoiceService.GetInvoicesByCustomerAsync(customerId);
            return Ok(invoices);
        }

        [HttpPost("generate/{reservationId}/{customerId}")]
        public async Task<IActionResult> GenerateInvoice(int reservationId, int customerId)
        {
            var invoice = await _invoiceService.GenerateInvoiceAsync(reservationId, customerId);
            return Ok(invoice);
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var pdfBytes = await _invoiceService.GenerateInvoicePdfAsync(id);
            return File(pdfBytes, "application/pdf", $"invoice_{id}.pdf");
        }

        [HttpPost("{id}/send-gib")]
        public async Task<IActionResult> SendToGIB(int id)
        {
            var result = await _invoiceService.SendEInvoiceToGIBAsync(id);
            return Ok(new { success = result, message = result ? "Sent to GIB successfully" : "Failed to send to GIB" });
        }
    }
}