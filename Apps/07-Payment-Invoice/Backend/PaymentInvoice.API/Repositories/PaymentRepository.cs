using PaymentInvoice.API.Models;
using PaymentInvoice.API.Data;

namespace PaymentInvoice.API.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly List<Payment> _payments;

        public PaymentRepository()
        {
            _payments = MockData.GetPayments();
        }

        public async Task<Payment> GetByIdAsync(int id)
        {
            return await Task.FromResult(_payments.FirstOrDefault(p => p.Id == id));
        }

        public async Task<Payment> GetByTransactionIdAsync(string transactionId)
        {
            return await Task.FromResult(_payments.FirstOrDefault(p => p.TransactionId == transactionId));
        }

        public async Task<Payment> GetByPaymentNumberAsync(string paymentNumber)
        {
            return await Task.FromResult(_payments.FirstOrDefault(p => p.PaymentNumber == paymentNumber));
        }

        public async Task<List<Payment>> GetByReservationIdAsync(int reservationId)
        {
            return await Task.FromResult(_payments.Where(p => p.ReservationId == reservationId).ToList());
        }

        public async Task<List<Payment>> GetByCustomerIdAsync(int customerId)
        {
            return await Task.FromResult(_payments.Where(p => p.CustomerId == customerId).ToList());
        }

        public async Task<List<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(_payments.Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate).ToList());
        }

        public async Task<List<Payment>> GetByStatusAsync(string status)
        {
            return await Task.FromResult(_payments.Where(p => p.Status == status).ToList());
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            payment.Id = _payments.Max(p => p.Id) + 1;
            _payments.Add(payment);
            return await Task.FromResult(payment);
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            var existing = await GetByIdAsync(payment.Id);
            if (existing != null)
            {
                var index = _payments.IndexOf(existing);
                _payments[index] = payment;
            }
            return await Task.FromResult(payment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await GetByIdAsync(id);
            if (payment != null)
            {
                _payments.Remove(payment);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _payments.Where(p => p.Status == "Success");
            if (startDate.HasValue)
                query = query.Where(p => p.PaymentDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(p => p.PaymentDate <= endDate.Value);
            
            return await Task.FromResult(query.Sum(p => p.Amount));
        }

        public async Task<Dictionary<string, decimal>> GetRevenueByPaymentMethodAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _payments.Where(p => p.Status == "Success");
            if (startDate.HasValue)
                query = query.Where(p => p.PaymentDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(p => p.PaymentDate <= endDate.Value);
            
            return await Task.FromResult(query.GroupBy(p => p.PaymentMethod)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.Amount)));
        }

        public async Task<int> GetPaymentCountByStatusAsync(string status)
        {
            return await Task.FromResult(_payments.Count(p => p.Status == status));
        }
    }
}