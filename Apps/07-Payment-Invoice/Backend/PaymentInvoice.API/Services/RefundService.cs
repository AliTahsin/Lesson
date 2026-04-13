using AutoMapper;
using PaymentInvoice.API.Models;
using PaymentInvoice.API.DTOs;
using PaymentInvoice.API.Repositories;

namespace PaymentInvoice.API.Services
{
    public class RefundService : IRefundService
    {
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IIyzicoService _iyzicoService;
        private readonly IMapper _mapper;
        private readonly ILogger<RefundService> _logger;

        public RefundService(
            IRefundRepository refundRepository,
            IPaymentRepository paymentRepository,
            IIyzicoService iyzicoService,
            IMapper mapper,
            ILogger<RefundService> logger)
        {
            _refundRepository = refundRepository;
            _paymentRepository = paymentRepository;
            _iyzicoService = iyzicoService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RefundResponseDto> ProcessRefundAsync(RefundRequestDto request)
        {
            var payment = await _paymentRepository.GetByIdAsync(request.PaymentId);
            if (payment == null)
                throw new Exception("Payment not found");

            if (payment.Status != "Success")
                throw new Exception("Cannot refund failed payment");

            if (request.Amount > payment.Amount)
                throw new Exception("Refund amount cannot exceed payment amount");

            var refund = new Refund
            {
                RefundNumber = $"REF-{DateTime.Now.Ticks}",
                PaymentId = request.PaymentId,
                ReservationId = request.ReservationId,
                Amount = request.Amount,
                Currency = payment.Currency,
                Reason = request.Reason,
                Status = "Pending",
                RequestDate = DateTime.Now,
                Notes = request.Notes
            };

            await _refundRepository.AddAsync(refund);

            var refundResponse = await _iyzicoService.CreateRefundAsync(request.PaymentId, request.Amount, request.Reason);
            
            if (refundResponse.Success)
            {
                refund.Status = "Processed";
                refund.TransactionId = refundResponse.RefundTransactionId;
                refund.ProcessedDate = refundResponse.ProcessedDate;
                await _refundRepository.UpdateAsync(refund);
                
                // Update payment status
                if (request.Amount >= payment.Amount)
                    payment.Status = "Refunded";
                else
                    payment.Status = "PartialRefund";
                
                payment.UpdatedAt = DateTime.Now;
                await _paymentRepository.UpdateAsync(payment);
            }
            else
            {
                refund.Status = "Failed";
                await _refundRepository.UpdateAsync(refund);
            }

            return new RefundResponseDto
            {
                Success = refundResponse.Success,
                RefundNumber = refund.RefundNumber,
                Amount = refund.Amount,
                Message = refundResponse.Message,
                ProcessedDate = refund.ProcessedDate
            };
        }

        public async Task<List<RefundDto>> GetRefundsByPaymentAsync(int paymentId)
        {
            var refunds = await _refundRepository.GetByPaymentIdAsync(paymentId);
            return _mapper.Map<List<RefundDto>>(refunds);
        }

        public async Task<List<RefundDto>> GetRefundsByReservationAsync(int reservationId)
        {
            var refunds = await _refundRepository.GetByReservationIdAsync(reservationId);
            return _mapper.Map<List<RefundDto>>(refunds);
        }

        public async Task<RefundStatisticsDto> GetRefundStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var refunds = await _refundRepository.GetByDateRangeAsync(startDate ?? DateTime.Now.AddMonths(-1), endDate ?? DateTime.Now);
            var processedRefunds = refunds.Where(r => r.Status == "Processed").ToList();
            
            return new RefundStatisticsDto
            {
                TotalRefunds = refunds.Count,
                TotalAmount = processedRefunds.Sum(r => r.Amount),
                ProcessedRefunds = processedRefunds.Count,
                FailedRefunds = refunds.Count(r => r.Status == "Failed"),
                PendingRefunds = refunds.Count(r => r.Status == "Pending"),
                AverageRefundAmount = processedRefunds.Any() ? processedRefunds.Average(r => r.Amount) : 0,
                ByReason = processedRefunds.GroupBy(r => r.Reason)
                    .Select(g => new RefundReasonStatDto
                    {
                        Reason = g.Key,
                        Count = g.Count(),
                        Amount = g.Sum(r => r.Amount)
                    }).ToList()
            };
        }
    }
}