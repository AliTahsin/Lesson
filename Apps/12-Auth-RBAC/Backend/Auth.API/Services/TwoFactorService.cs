using Auth.API.Models;

namespace Auth.API.Services
{
    public class TwoFactorService : ITwoFactorService
    {
        private readonly List<TwoFactorCode> _codes;
        private readonly ILogger<TwoFactorService> _logger;

        public TwoFactorService(ILogger<TwoFactorService> logger)
        {
            _codes = new List<TwoFactorCode>();
            _logger = logger;
        }

        public async Task<string> GenerateAndSendCodeAsync(int userId, string method)
        {
            var code = new Random().Next(100000, 999999).ToString();
            
            // Remove old codes
            var oldCodes = _codes.Where(c => c.UserId == userId).ToList();
            foreach (var oldCode in oldCodes)
                _codes.Remove(oldCode);
            
            var twoFactorCode = new TwoFactorCode
            {
                Id = _codes.Count + 1,
                UserId = userId,
                Code = code,
                Method = method,
                IsUsed = false,
                ExpiryDate = DateTime.Now.AddMinutes(5),
                CreatedAt = DateTime.Now
            };
            
            _codes.Add(twoFactorCode);
            
            // Send code via specified method
            if (method == "SMS")
            {
                await SendSmsCodeAsync($"+90{new Random().Next(500000000, 599999999)}", code);
            }
            else if (method == "Email")
            {
                await SendEmailCodeAsync($"user{userId}@example.com", code);
            }
            
            _logger.LogInformation($"2FA code for user {userId}: {code}");
            return code;
        }

        public async Task<bool> VerifyCodeAsync(int userId, string code)
        {
            var twoFactorCode = _codes.FirstOrDefault(c => c.UserId == userId && c.Code == code && !c.IsUsed);
            
            if (twoFactorCode == null)
                return await Task.FromResult(false);
            
            if (twoFactorCode.ExpiryDate < DateTime.Now)
                return await Task.FromResult(false);
            
            twoFactorCode.IsUsed = true;
            return await Task.FromResult(true);
        }

        public async Task<bool> SendSmsCodeAsync(string phoneNumber, string code)
        {
            // Simulate SMS sending
            _logger.LogInformation($"SMS sent to {phoneNumber}: Your verification code is {code}");
            await Task.Delay(100);
            return true;
        }

        public async Task<bool> SendEmailCodeAsync(string email, string code)
        {
            // Simulate email sending
            _logger.LogInformation($"Email sent to {email}: Your verification code is {code}");
            await Task.Delay(100);
            return true;
        }
    }
}