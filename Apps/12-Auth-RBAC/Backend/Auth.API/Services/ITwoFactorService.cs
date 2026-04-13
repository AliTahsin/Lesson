namespace Auth.API.Services
{
    public interface ITwoFactorService
    {
        Task<string> GenerateAndSendCodeAsync(int userId, string method);
        Task<bool> VerifyCodeAsync(int userId, string code);
        Task<bool> SendSmsCodeAsync(string phoneNumber, string code);
        Task<bool> SendEmailCodeAsync(string email, string code);
    }
}