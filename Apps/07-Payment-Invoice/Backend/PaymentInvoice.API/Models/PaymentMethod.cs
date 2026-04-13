namespace PaymentInvoice.API.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string MethodType { get; set; } // CreditCard, BankAccount
        public string MaskedCardNumber { get; set; }
        public string CardBrand { get; set; }
        public string CardHolderName { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Token { get; set; } // Tokenized card
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}