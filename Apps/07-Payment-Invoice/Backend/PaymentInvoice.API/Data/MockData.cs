using PaymentInvoice.API.Models;

namespace PaymentInvoice.API.Data
{
    public static class MockData
    {
        private static readonly Random _random = new Random();

        public static List<Payment> GetPayments()
        {
            var payments = new List<Payment>();
            
            for (int i = 1; i <= 200; i++)
            {
                var statuses = new[] { "Success", "Success", "Success", "Failed", "Refunded" };
                var methods = new[] { "CreditCard", "CreditCard", "PayPal", "BankTransfer" };
                var brands = new[] { "Visa", "Mastercard", "American Express" };
                
                var status = statuses[_random.Next(statuses.Length)];
                var isSuccess = status == "Success";
                var paymentDate = DateTime.Now.AddDays(-_random.Next(0, 90));
                
                payments.Add(new Payment
                {
                    Id = i,
                    PaymentNumber = $"PAY-{DateTime.Now.Year}-{i:D6}",
                    ReservationId = _random.Next(1, 500),
                    CustomerId = _random.Next(1, 100),
                    Amount = _random.Next(100, 5000),
                    Currency = "EUR",
                    PaymentMethod = methods[_random.Next(methods.Length)],
                    CardBrand = isSuccess ? brands[_random.Next(brands.Length)] : null,
                    MaskedCardNumber = isSuccess ? $"**** **** **** {_random.Next(1000, 9999)}" : null,
                    CardHolderName = isSuccess ? $"CARD HOLDER {i}" : null,
                    Installment = _random.Next(1, 13).ToString(),
                    TransactionId = isSuccess ? $"TXN-{DateTime.Now.Ticks}-{i}" : null,
                    AuthCode = isSuccess ? _random.Next(100000, 999999).ToString() : null,
                    HostReference = isSuccess ? $"REF-{DateTime.Now.Ticks}-{i}" : null,
                    Status = status,
                    PaymentDate = paymentDate,
                    SuccessDate = isSuccess ? paymentDate : null,
                    PaymentSource = _random.Next(0, 10) > 7 ? "Mobile" : "Web",
                    CreatedAt = paymentDate,
                    UpdatedAt = isSuccess ? paymentDate : null
                });
            }
            
            return payments;
        }

        public static List<Invoice> GetInvoices()
        {
            var invoices = new List<Invoice>();
            
            for (int i = 1; i <= 150; i++)
            {
                var subTotal = _random.Next(500, 5000);
                var taxRate = 20;
                var taxAmount = subTotal * taxRate / 100;
                var totalAmount = subTotal + taxAmount;
                var issueDate = DateTime.Now.AddDays(-_random.Next(0, 90));
                
                var invoice = new Invoice
                {
                    Id = i,
                    InvoiceNumber = $"INV-{DateTime.Now.Year}-{i:D6}",
                    EInvoiceNumber = i % 10 != 0 ? $"EINV-{DateTime.Now.Ticks}-{i}" : null,
                    ReservationId = i,
                    CustomerId = _random.Next(1, 100),
                    CustomerName = $"Customer {_random.Next(1, 100)}",
                    CustomerTaxId = $"{_random.Next(1000000000, 9999999999)}",
                    CustomerTaxOffice = _random.Next(0, 10) > 7 ? "Vergi Dairesi" : null,
                    SubTotal = subTotal,
                    TaxRate = taxRate,
                    TaxAmount = taxAmount,
                    TotalAmount = totalAmount,
                    Currency = "EUR",
                    IssueDate = issueDate,
                    DueDate = issueDate.AddDays(7),
                    Status = _random.Next(0, 10) > 8 ? "Paid" : "Issued",
                    Type = "Standard",
                    GIBStatus = _random.Next(0, 10) > 2 ? "Sent" : "Pending",
                    CreatedAt = issueDate,
                    Items = new List<InvoiceItem>
                    {
                        new InvoiceItem
                        {
                            Id = i * 10,
                            InvoiceId = i,
                            Description = "Otel Konaklama",
                            Quantity = _random.Next(1, 7),
                            UnitPrice = _random.Next(100, 500),
                            TotalPrice = 0,
                            TaxRate = taxRate,
                            TaxAmount = 0
                        }
                    }
                };

                foreach (var item in invoice.Items)
                {
                    item.TotalPrice = item.Quantity * item.UnitPrice;
                    item.TaxAmount = item.TotalPrice * (item.TaxRate / 100);
                }

                invoices.Add(invoice);
            }
            
            return invoices;
        }

        public static List<Refund> GetRefunds()
        {
            var refunds = new List<Refund>();
            
            for (int i = 1; i <= 50; i++)
            {
                refunds.Add(new Refund
                {
                    Id = i,
                    RefundNumber = $"REF-{DateTime.Now.Year}-{i:D6}",
                    PaymentId = _random.Next(1, 200),
                    ReservationId = _random.Next(1, 500),
                    Amount = _random.Next(50, 2000),
                    Currency = "EUR",
                    Reason = _random.Next(0, 10) > 7 ? "CustomerRequest" : "Cancellation",
                    Status = _random.Next(0, 10) > 8 ? "Failed" : "Processed",
                    TransactionId = $"RFND-{DateTime.Now.Ticks}-{i}",
                    RequestDate = DateTime.Now.AddDays(-_random.Next(0, 30)),
                    ProcessedDate = DateTime.Now.AddDays(-_random.Next(0, 30)),
                    ProcessedBy = "System",
                    Notes = null
                });
            }
            
            return refunds;
        }
    }
}