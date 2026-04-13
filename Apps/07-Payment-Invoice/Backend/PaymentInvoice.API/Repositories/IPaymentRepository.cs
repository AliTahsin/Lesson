using PaymentInvoice.API.Models;

namespace PaymentInvoice.API.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(int id);
        Task<Payment> GetByTransactionIdAsync(string transactionId);
        Task<Payment> GetByPaymentNumberAsync(string paymentNumber);
        Task<List<Payment>> GetByReservationIdAsync(int reservationId);
        Task<List<Payment>> GetByCustomerIdAsync(int customerId);
        Task<List<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Payment>> GetByStatusAsync(string status);
        Task<Payment> AddAsync(Payment payment);
        Task<Payment> UpdateAsync(Payment payment);
        Task<bool> DeleteAsync(int id);
        Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Dictionary<string, decimal>> GetRevenueByPaymentMethodAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<int> GetPaymentCountByStatusAsync(string status);
    }
}