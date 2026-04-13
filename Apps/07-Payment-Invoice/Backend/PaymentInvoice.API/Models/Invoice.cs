using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentInvoice.API.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string InvoiceNumber { get; set; }
        
        public string EInvoiceNumber { get; set; } // GİB e-Fatura numarası
        
        [Required]
        public int ReservationId { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        
        [Required]
        public string CustomerName { get; set; }
        
        [Required]
        public string CustomerTaxId { get; set; } // Vergi No / TCKN
        
        public string CustomerTaxOffice { get; set; }
        
        public string CustomerAddress { get; set; }
        
        public string CustomerEmail { get; set; }
        
        public string CustomerPhone { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }
        
        public decimal TaxRate { get; set; } // 1, 10, 20
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        public string Currency { get; set; }
        
        public DateTime IssueDate { get; set; }
        
        public DateTime DueDate { get; set; }
        
        public DateTime? PaymentDate { get; set; }
        
        public string Status { get; set; } // Draft, Issued, Paid, Cancelled, EInvoiceSent
        
        public string Type { get; set; } // Standard, Proforma, Cancellation, Refund
        
        public string GIBStatus { get; set; } // Pending, Sent, Approved, Rejected
        
        public string GIBErrorMessage { get; set; }
        
        public string PdfUrl { get; set; }
        
        public string XmlUrl { get; set; }
        
        public List<InvoiceItem> Items { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }

    public class InvoiceItem
    {
        [Key]
        public int Id { get; set; }
        
        public int InvoiceId { get; set; }
        
        public string Description { get; set; }
        
        public int Quantity { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        
        public decimal TaxRate { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }
        
        public string GtinCode { get; set; } // Ürün kodu
    }
}