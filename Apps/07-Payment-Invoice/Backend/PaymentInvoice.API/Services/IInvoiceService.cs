using PaymentInvoice.API.DTOs;

namespace PaymentInvoice.API.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> GetInvoiceByIdAsync(int id);
        Task<InvoiceDto> GetInvoiceByReservationAsync(int reservationId);
        Task<List<InvoiceDto>> GetInvoicesByCustomerAsync(int customerId);
        Task<InvoiceDto> GenerateInvoiceAsync(int reservationId, int customerId);
        Task<byte[]> GenerateInvoicePdfAsync(int invoiceId);
        Task<bool> SendEInvoiceToGIBAsync(int invoiceId);
    }
}