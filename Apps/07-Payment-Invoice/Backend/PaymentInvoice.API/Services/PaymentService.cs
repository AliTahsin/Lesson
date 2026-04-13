using AutoMapper;
using PaymentInvoice.API.Models;
using PaymentInvoice.API.DTOs;
using PaymentInvoice.API.Repositories;
using PaymentInvoice.API.Security;

namespace PaymentInvoice.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInvoiceService _invoiceService;
        private readonly IEncryptionService _encryption;
        private readonly IIyzicoService _iyzicoService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IInvoiceService invoiceService,
            IEncryptionService encryption,
            IIyzicoService iyzicoService,
            IMapper mapper,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _invoiceService = invoiceService;
            _encryption = encryption;
            _iyzicoService = iyzicoService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request)
        {
            // Validate request
            if (request.Amount <= 0)
                throw new Exception("Amount must be greater than 0");

            // Mask card number for logging
            var maskedCard = _encryption.MaskCardNumber(request.CardNumber);
            _logger.LogInformation("Processing payment for reservation {ReservationId} with card {MaskedCard}", 
                request.ReservationId, maskedCard);

            // Create payment record
            var payment = new Payment
            {
                PaymentNumber = $"PAY-{DateTime.Now.Ticks}",
                ReservationId = request.ReservationId,
                CustomerId = request.CustomerId,
                Amount = request.Amount,
                Currency = request.Currency,
                PaymentMethod = request.PaymentMethod,
                CardBrand = GetCardBrand(request.CardNumber),
                MaskedCardNumber = maskedCard,
                CardHolderName = request.CardHolderName,
                Installment = request.Installment?.ToString() ?? "1",
                Status = "Pending",
                PaymentSource = request.PaymentSource,
                IpAddress = request.IpAddress,
                UserAgent = request.UserAgent,
                CreatedAt = DateTime.Now,
                Metadata = new Dictionary<string, string>
                {
                    { "Installment", request.Installment?.ToString() ?? "1" },
                    { "Gateway", "Iyzico" }
                }
            };

            await _paymentRepository.AddAsync(payment);

            // Process payment via Iyzico
            var paymentRequest = new PaymentRequest
            {
                Amount = request.Amount,
                Currency = request.Currency,
                CardNumber = request.CardNumber,
                CardHolderName = request.CardHolderName,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                Cvv = request.Cvv,
                Installment = request.Installment ?? 1,
                ReservationId = request.ReservationId
            };

            var iyzicoResponse = await _iyzicoService.CreatePaymentAsync(paymentRequest);

            // Update payment record
            payment.TransactionId = iyzicoResponse.TransactionId;
            payment.AuthCode = iyzicoResponse.AuthCode;
            payment.HostReference = iyzicoResponse.HostReference;
            
            if (iyzicoResponse.Success)
            {
                payment.Status = "Success";
                payment.SuccessDate = DateTime.Now;
                payment.ErrorCode = null;
                payment.ErrorMessage = null;
                
                // Generate invoice after successful payment
                await _invoiceService.GenerateInvoiceAsync(request.ReservationId, request.CustomerId);
            }
            else
            {
                payment.Status = "Failed";
                payment.ErrorCode = iyzicoResponse.ErrorCode;
                payment.ErrorMessage = iyzicoResponse.ErrorMessage;
            }
            
            payment.UpdatedAt = DateTime.Now;
            await _paymentRepository.UpdateAsync(payment);

            var response = _mapper.Map<PaymentResponseDto>(payment);
            response.IsSuccess = iyzicoResponse.Success;
            response.Message = iyzicoResponse.Message;
            
            return response;
        }

        public async Task<PaymentResponseDto> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return payment != null ? _mapper.Map<PaymentResponseDto>(payment) : null;
        }

        public async Task<List<PaymentResponseDto>> GetPaymentsByReservationAsync(int reservationId)
        {
            var payments = await _paymentRepository.GetByReservationIdAsync(reservationId);
            return _mapper.Map<List<PaymentResponseDto>>(payments);
        }

        public async Task<List<PaymentResponseDto>> GetPaymentsByCustomerAsync(int customerId)
        {
            var payments = await _paymentRepository.GetByCustomerIdAsync(customerId);
            return _mapper.Map<List<PaymentResponseDto>>(payments);
        }

        public async Task<PaymentStatisticsDto> GetPaymentStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var payments = await _paymentRepository.GetByDateRangeAsync(startDate ?? DateTime.Now.AddMonths(-1), endDate ?? DateTime.Now);
            var successfulPayments = payments.Where(p => p.Status == "Success").ToList();
            
            var stats = new PaymentStatisticsDto
            {
                TotalPayments = payments.Count,
                TotalAmount = successfulPayments.Sum(p => p.Amount),
                SuccessfulPayments = successfulPayments.Count,
                FailedPayments = payments.Count(p => p.Status == "Failed"),
                RefundedPayments = payments.Count(p => p.Status == "Refunded"),
                ByPaymentMethod = successfulPayments.GroupBy(p => p.PaymentMethod)
                    .Select(g => new PaymentMethodStatDto
                    {
                        Method = g.Key,
                        Count = g.Count(),
                        Amount = g.Sum(p => p.Amount)
                    }).ToList(),
                ByCardBrand = successfulPayments.Where(p => !string.IsNullOrEmpty(p.CardBrand))
                    .GroupBy(p => p.CardBrand)
                    .Select(g => new CardBrandStatDto
                    {
                        Brand = g.Key,
                        Count = g.Count(),
                        Amount = g.Sum(p => p.Amount)
                    }).ToList(),
                DailyTotals = successfulPayments.GroupBy(p => p.PaymentDate.Date)
                    .Select(g => new DailyPaymentDto
                    {
                        Date = g.Key,
                        Count = g.Count(),
                        Amount = g.Sum(p => p.Amount)
                    }).OrderBy(d => d.Date).ToList()
            };

            return stats;
        }

        public async Task<bool> CancelPaymentAsync(int paymentId, string reason)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
                throw new Exception("Payment not found");

            if (payment.Status != "Pending")
                throw new Exception("Only pending payments can be cancelled");

            var cancelResponse = await _iyzicoService.CancelPaymentAsync(payment.TransactionId);
            
            if (cancelResponse)
            {
                payment.Status = "Cancelled";
                payment.UpdatedAt = DateTime.Now;
                await _paymentRepository.UpdateAsync(payment);
                return true;
            }

            return false;
        }

        private string GetCardBrand(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber)) return "Unknown";
            
            var firstDigit = cardNumber[0].ToString();
            var firstTwo = cardNumber.Length >= 2 ? cardNumber[..2] : "";
            var firstFour = cardNumber.Length >= 4 ? cardNumber[..4] : "";
            
            if (firstDigit == "4") return "Visa";
            if (firstTwo is "51" or "52" or "53" or "54" or "55") return "Mastercard";
            if (firstTwo is "34" or "37") return "American Express";
            if (firstFour == "6011") return "Discover";
            
            return "Unknown";
        }
    }
}