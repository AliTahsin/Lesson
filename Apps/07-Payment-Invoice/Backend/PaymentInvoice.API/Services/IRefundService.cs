using PaymentInvoice.API.DTOs;

namespace PaymentInvoice.API.Services
{
    public interface IRefundService
    {
        Task<RefundResponseDto> ProcessRefundAsync(RefundRequestDto request);
        Task<List<RefundDto>> GetRefundsByPaymentAsync(int paymentId);
        Task<List<RefundDto>> GetRefundsByReservationAsync(int reservationId);
        Task<RefundStatisticsDto> GetRefundStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}