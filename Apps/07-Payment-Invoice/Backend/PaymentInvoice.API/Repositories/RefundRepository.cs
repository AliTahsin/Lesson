using PaymentInvoice.API.Models;
using PaymentInvoice.API.Data;

namespace PaymentInvoice.API.Repositories
{
    public class RefundRepository : IRefundRepository
    {
        private readonly List<Refund> _refunds;

        public RefundRepository()
        {
            _refunds = MockData.GetRefunds();
        }

        public async Task<Refund> GetByIdAsync(int id)
        {
            return await Task.FromResult(_refunds.FirstOrDefault(r => r.Id == id));
        }

        public async Task<Refund> GetByRefundNumberAsync(string refundNumber)
        {
            return await Task.FromResult(_refunds.FirstOrDefault(r => r.RefundNumber == refundNumber));
        }

        public async Task<List<Refund>> GetByPaymentIdAsync(int paymentId)
        {
            return await Task.FromResult(_refunds.Where(r => r.PaymentId == paymentId).ToList());
        }

        public async Task<List<Refund>> GetByReservationIdAsync(int reservationId)
        {
            return await Task.FromResult(_refunds.Where(r => r.ReservationId == reservationId).ToList());
        }

        public async Task<List<Refund>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(_refunds.Where(r => r.RequestDate >= startDate && r.RequestDate <= endDate).ToList());
        }

        public async Task<Refund> AddAsync(Refund refund)
        {
            refund.Id = _refunds.Max(r => r.Id) + 1;
            _refunds.Add(refund);
            return await Task.FromResult(refund);
        }

        public async Task<Refund> UpdateAsync(Refund refund)
        {
            var existing = await GetByIdAsync(refund.Id);
            if (existing != null)
            {
                var index = _refunds.IndexOf(existing);
                _refunds[index] = refund;
            }
            return await Task.FromResult(refund);
        }

        public async Task<decimal> GetTotalRefundedAmountAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _refunds.Where(r => r.Status == "Processed");
            if (startDate.HasValue)
                query = query.Where(r => r.RequestDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(r => r.RequestDate <= endDate.Value);
            
            return await Task.FromResult(query.Sum(r => r.Amount));
        }
    }
}