using MobileCustomer.API.Models;

namespace MobileCustomer.API.Repositories
{
    public interface IProfileRepository
    {
        Task<CustomerProfile> GetByUserIdAsync(int userId);
        Task<CustomerProfile> AddAsync(CustomerProfile profile);
        Task<CustomerProfile> UpdateAsync(CustomerProfile profile);
        Task<bool> UpdateBiometricAsync(int userId, bool enabled, string biometricKey);
        Task<bool> UpdateLanguageAsync(int userId, string language);
        Task<bool> UpdateNotificationSettingsAsync(int userId, bool push, bool email, bool sms);
    }
}