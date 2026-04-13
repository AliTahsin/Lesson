using PaymentInvoice.API.Models;

namespace PaymentInvoice.API.Repositories
{
    public interface IRefundRepository
    {
        Task<Refund> GetByIdAsync(int id);
        Task<Refund> GetByRefundNumberAsync(string refundNumber);
        Task<List<Refund>> GetByPaymentIdAsync(int paymentId);
        Task<List<Refund>> GetByReservationIdAsync(int reservationId);
        Task<List<Refund>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Refund> AddAsync(Refund refund);
        Task<Refund> UpdateAsync(Refund refund);
        Task<decimal> GetTotalRefundedAmountAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}