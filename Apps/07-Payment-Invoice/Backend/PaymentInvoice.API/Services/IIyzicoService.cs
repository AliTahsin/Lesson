using PaymentInvoice.API.DTOs;

namespace PaymentInvoice.API.Services
{
    public interface IIyzicoService
    {
        Task<IyzicoPaymentResponse> CreatePaymentAsync(PaymentRequest request);
        Task<IyzicoRefundResponse> CreateRefundAsync(int paymentId, decimal amount, string reason);
        Task<IyzicoPaymentStatusResponse> CheckPaymentStatusAsync(string transactionId);
        Task<bool> CancelPaymentAsync(string transactionId);
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; set; }
        public int Installment { get; set; }
        public int ReservationId { get; set; }
    }

    public class IyzicoPaymentResponse
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string AuthCode { get; set; }
        public string HostReference { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class IyzicoRefundResponse
    {
        public bool Success { get; set; }
        public string RefundTransactionId { get; set; }
        public string Message { get; set; }
        public DateTime ProcessedDate { get; set; }
    }

    public class IyzicoPaymentStatusResponse
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}