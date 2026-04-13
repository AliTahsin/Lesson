using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentInvoice.API.Models
{
    public class Refund
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string RefundNumber { get; set; }
        
        [Required]
        public int PaymentId { get; set; }
        
        [Required]
        public int ReservationId { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        public string Currency { get; set; }
        
        public string Reason { get; set; } // Cancellation, Overpayment, CustomerRequest
        
        public string Status { get; set; } // Pending, Processed, Failed
        
        public string TransactionId { get; set; }
        
        public DateTime RequestDate { get; set; }
        
        public DateTime? ProcessedDate { get; set; }
        
        public string ProcessedBy { get; set; }
        
        public string Notes { get; set; }
    }
}