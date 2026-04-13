namespace PaymentInvoice.API.DTOs
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }
        public string PaymentNumber { get; set; }
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentMethod { get; set; }
        public string CardBrand { get; set; }
        public string MaskedCardNumber { get; set; }
        public string Installment { get; set; }
        public string Status { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string AuthCode { get; set; }
    }
}