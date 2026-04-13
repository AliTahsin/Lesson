using PaymentInvoice.API.DTOs;

namespace PaymentInvoice.API.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request);
        Task<PaymentResponseDto> GetPaymentByIdAsync(int id);
        Task<List<PaymentResponseDto>> GetPaymentsByReservationAsync(int reservationId);
        Task<List<PaymentResponseDto>> GetPaymentsByCustomerAsync(int customerId);
        Task<PaymentStatisticsDto> GetPaymentStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<bool> CancelPaymentAsync(int paymentId, string reason);
    }
}