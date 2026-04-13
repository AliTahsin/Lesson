using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentInvoice.API.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string PaymentNumber { get; set; }
        
        [Required]
        public int ReservationId { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        [Required]
        public string Currency { get; set; } // TRY, USD, EUR, GBP
        
        [Required]
        public string PaymentMethod { get; set; } // CreditCard, PayPal, BankTransfer, Cash
        
        public string CardBrand { get; set; } // Visa, Mastercard, Amex
        
        public string MaskedCardNumber { get; set; } // **** **** **** 1234
        
        public string CardHolderName { get; set; }
        
        public string Installment { get; set; } // 1, 2, 3, 6, 9, 12
        
        public string TransactionId { get; set; } // Bank/Provider transaction ID
        
        public string AuthCode { get; set; }
        
        public string HostReference { get; set; }
        
        public string Status { get; set; } // Pending, Success, Failed, Refunded, PartialRefund
        
        public string ErrorCode { get; set; }
        
        public string ErrorMessage { get; set; }
        
        public DateTime PaymentDate { get; set; }
        
        public DateTime? SuccessDate { get; set; }
        
        public string PaymentSource { get; set; } // Web, Mobile, POS, API
        
        public string IpAddress { get; set; }
        
        public string UserAgent { get; set; }
        
        public Dictionary<string, string> Metadata { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}