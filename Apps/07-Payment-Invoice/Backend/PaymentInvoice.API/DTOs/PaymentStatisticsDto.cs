namespace PaymentInvoice.API.DTOs
{
    public class PaymentStatisticsDto
    {
        public int TotalPayments { get; set; }
        public decimal TotalAmount { get; set; }
        public int SuccessfulPayments { get; set; }
        public int FailedPayments { get; set; }
        public int RefundedPayments { get; set; }
        public List<PaymentMethodStatDto> ByPaymentMethod { get; set; }
        public List<CardBrandStatDto> ByCardBrand { get; set; }
        public List<DailyPaymentDto> DailyTotals { get; set; }
    }

    public class PaymentMethodStatDto
    {
        public string Method { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }

    public class CardBrandStatDto
    {
        public string Brand { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }

    public class DailyPaymentDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }

    public class RefundStatisticsDto
    {
        public int TotalRefunds { get; set; }
        public decimal TotalAmount { get; set; }
        public int ProcessedRefunds { get; set; }
        public int FailedRefunds { get; set; }
        public int PendingRefunds { get; set; }
        public decimal AverageRefundAmount { get; set; }
        public List<RefundReasonStatDto> ByReason { get; set; }
    }

    public class RefundReasonStatDto
    {
        public string Reason { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }
}