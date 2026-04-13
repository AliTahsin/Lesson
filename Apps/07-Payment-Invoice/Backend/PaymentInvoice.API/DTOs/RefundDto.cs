namespace PaymentInvoice.API.DTOs
{
    public class RefundRequestDto
    {
        public int PaymentId { get; set; }
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
    }

    public class RefundResponseDto
    {
        public bool Success { get; set; }
        public string RefundNumber { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }

    public class RefundDto
    {
        public int Id { get; set; }
        public string RefundNumber { get; set; }
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}