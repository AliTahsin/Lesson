using PaymentInvoice.API.Models;
using PaymentInvoice.API.Data;

namespace PaymentInvoice.API.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly List<Invoice> _invoices;

        public InvoiceRepository()
        {
            _invoices = MockData.GetInvoices();
        }

        public async Task<Invoice> GetByIdAsync(int id)
        {
            return await Task.FromResult(_invoices.FirstOrDefault(i => i.Id == id));
        }

        public async Task<Invoice> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            return await Task.FromResult(_invoices.FirstOrDefault(i => i.InvoiceNumber == invoiceNumber));
        }

        public async Task<Invoice> GetByReservationIdAsync(int reservationId)
        {
            return await Task.FromResult(_invoices.FirstOrDefault(i => i.ReservationId == reservationId));
        }

        public async Task<List<Invoice>> GetByCustomerIdAsync(int customerId)
        {
            return await Task.FromResult(_invoices.Where(i => i.CustomerId == customerId).ToList());
        }

        public async Task<List<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(_invoices.Where(i => i.IssueDate >= startDate && i.IssueDate <= endDate).ToList());
        }

        public async Task<List<Invoice>> GetByStatusAsync(string status)
        {
            return await Task.FromResult(_invoices.Where(i => i.Status == status).ToList());
        }

        public async Task<Invoice> AddAsync(Invoice invoice)
        {
            invoice.Id = _invoices.Max(i => i.Id) + 1;
            _invoices.Add(invoice);
            return await Task.FromResult(invoice);
        }

        public async Task<Invoice> UpdateAsync(Invoice invoice)
        {
            var existing = await GetByIdAsync(invoice.Id);
            if (existing != null)
            {
                var index = _invoices.IndexOf(existing);
                _invoices[index] = invoice;
            }
            return await Task.FromResult(invoice);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var invoice = await GetByIdAsync(id);
            if (invoice != null)
            {
                _invoices.Remove(invoice);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<Invoice> GenerateInvoiceAsync(int reservationId, int customerId)
        {
            // Simulate invoice generation
            var random = new Random();
            var invoice = new Invoice
            {
                Id = _invoices.Max(i => i.Id) + 1,
                InvoiceNumber = $"INV-{DateTime.Now.Year}-{_invoices.Count + 1:D6}",
                EInvoiceNumber = $"EINV-{DateTime.Now.Ticks}",
                ReservationId = reservationId,
                CustomerId = customerId,
                CustomerName = $"Customer {customerId}",
                CustomerTaxId = $"{random.Next(1000000000, 9999999999)}",
                SubTotal = random.Next(500, 5000),
                TaxRate = 20,
                TaxAmount = 0,
                TotalAmount = 0,
                Currency = "EUR",
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                Status = "Issued",
                Type = "Standard",
                GIBStatus = "Pending",
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem
                    {
                        Description = "Otel Konaklama",
                        Quantity = random.Next(1, 7),
                        UnitPrice = random.Next(100, 500),
                        TotalPrice = 0,
                        TaxRate = 20,
                        TaxAmount = 0
                    }
                },
                CreatedAt = DateTime.Now
            };

            foreach (var item in invoice.Items)
            {
                item.TotalPrice = item.Quantity * item.UnitPrice;
                item.TaxAmount = item.TotalPrice * (item.TaxRate / 100);
                invoice.SubTotal += item.TotalPrice;
                invoice.TaxAmount += item.TaxAmount;
            }
            invoice.TotalAmount = invoice.SubTotal + invoice.TaxAmount;

            _invoices.Add(invoice);
            return await Task.FromResult(invoice);
        }
    }
}