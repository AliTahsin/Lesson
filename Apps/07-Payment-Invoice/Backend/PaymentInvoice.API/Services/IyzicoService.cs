namespace PaymentInvoice.API.Services
{
    public class IyzicoService : IIyzicoService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IyzicoService> _logger;

        public IyzicoService(IConfiguration configuration, ILogger<IyzicoService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IyzicoPaymentResponse> CreatePaymentAsync(PaymentRequest request)
        {
            try
            {
                // Simulate iyzico API call
                await Task.Delay(500);
                
                var random = new Random();
                var isSuccess = random.Next(0, 10) < 9; // 90% success rate
                
                if (isSuccess)
                {
                    return new IyzicoPaymentResponse
                    {
                        Success = true,
                        TransactionId = $"IYZ-{DateTime.Now.Ticks}-{random.Next(1000, 9999)}",
                        AuthCode = random.Next(100000, 999999).ToString(),
                        HostReference = $"REF-{DateTime.Now.Ticks}",
                        Message = "Payment successful"
                    };
                }
                else
                {
                    var errors = new[] { "Insufficient funds", "Card expired", "Invalid CVV", "Card declined", "3D Secure failed" };
                    return new IyzicoPaymentResponse
                    {
                        Success = false,
                        ErrorCode = $"ERR-{random.Next(100, 999)}",
                        ErrorMessage = errors[random.Next(errors.Length)],
                        Message = "Payment failed"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment via Iyzico");
                return new IyzicoPaymentResponse
                {
                    Success = false,
                    ErrorCode = "SYS-001",
                    ErrorMessage = "System error occurred",
                    Message = "Payment failed due to system error"
                };
            }
        }

        public async Task<IyzicoRefundResponse> CreateRefundAsync(int paymentId, decimal amount, string reason)
        {
            await Task.Delay(300);
            
            var random = new Random();
            return new IyzicoRefundResponse
            {
                Success = true,
                RefundTransactionId = $"RFND-{DateTime.Now.Ticks}",
                Message = "Refund processed successfully",
                ProcessedDate = DateTime.Now
            };
        }

        public async Task<IyzicoPaymentStatusResponse> CheckPaymentStatusAsync(string transactionId)
        {
            await Task.Delay(200);
            
            var random = new Random();
            return new IyzicoPaymentStatusResponse
            {
                TransactionId = transactionId,
                Status = random.Next(0, 10) < 8 ? "Success" : "Failed",
                Amount = random.Next(100, 1000),
                PaymentDate = DateTime.Now.AddMinutes(-random.Next(1, 60))
            };
        }

        public async Task<bool> CancelPaymentAsync(string transactionId)
        {
            await Task.Delay(200);
            return true;
        }
    }
}