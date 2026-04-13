using System.ComponentModel.DataAnnotations;

namespace PaymentInvoice.API.DTOs
{
    public class PaymentRequestDto
    {
        [Required]
        public int ReservationId { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        
        [Required]
        [Range(0.01, 999999.99)]
        public decimal Amount { get; set; }
        
        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "EUR";
        
        [Required]
        public string PaymentMethod { get; set; } = "CreditCard";
        
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }
        
        [Required]
        public string CardHolderName { get; set; }
        
        [Required]
        [Range(1, 12)]
        public int ExpiryMonth { get; set; }
        
        [Required]
        [Range(2024, 2035)]
        public int ExpiryYear { get; set; }
        
        [Required]
        [MinLength(3)]
        [MaxLength(4)]
        public string Cvv { get; set; }
        
        [Range(1, 12)]
        public int? Installment { get; set; } = 1;
        
        public string PaymentSource { get; set; } = "Web";
        
        public string IpAddress { get; set; }
        
        public string UserAgent { get; set; }
    }
}