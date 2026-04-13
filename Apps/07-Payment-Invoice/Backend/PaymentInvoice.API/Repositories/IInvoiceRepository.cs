using PaymentInvoice.API.Models;

namespace PaymentInvoice.API.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetByIdAsync(int id);
        Task<Invoice> GetByInvoiceNumberAsync(string invoiceNumber);
        Task<Invoice> GetByReservationIdAsync(int reservationId);
        Task<List<Invoice>> GetByCustomerIdAsync(int customerId);
        Task<List<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Invoice>> GetByStatusAsync(string status);
        Task<Invoice> AddAsync(Invoice invoice);
        Task<Invoice> UpdateAsync(Invoice invoice);
        Task<bool> DeleteAsync(int id);
        Task<Invoice> GenerateInvoiceAsync(int reservationId, int customerId);
    }
}