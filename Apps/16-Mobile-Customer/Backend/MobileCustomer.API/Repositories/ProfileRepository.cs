using MobileCustomer.API.Models;
using MobileCustomer.API.Data;

namespace MobileCustomer.API.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly List<CustomerProfile> _profiles;

        public ProfileRepository()
        {
            _profiles = MockData.GetProfiles();
        }

        public async Task<CustomerProfile> GetByUserIdAsync(int userId)
        {
            return await Task.FromResult(_profiles.FirstOrDefault(p => p.UserId == userId));
        }

        public async Task<CustomerProfile> AddAsync(CustomerProfile profile)
        {
            profile.Id = _profiles.Max(p => p.Id) + 1;
            profile.CreatedAt = DateTime.Now;
            _profiles.Add(profile);
            return await Task.FromResult(profile);
        }

        public async Task<CustomerProfile> UpdateAsync(CustomerProfile profile)
        {
            var existing = await GetByUserIdAsync(profile.UserId);
            if (existing != null)
            {
                var index = _profiles.IndexOf(existing);
                profile.UpdatedAt = DateTime.Now;
                _profiles[index] = profile;
            }
            return await Task.FromResult(profile);
        }

        public async Task<bool> UpdateBiometricAsync(int userId, bool enabled, string biometricKey)
        {
            var profile = await GetByUserIdAsync(userId);
            if (profile != null)
            {
                profile.BiometricEnabled = enabled;
                profile.BiometricKey = enabled ? biometricKey : null;
                profile.UpdatedAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateLanguageAsync(int userId, string language)
        {
            var profile = await GetByUserIdAsync(userId);
            if (profile != null)
            {
                profile.Language = language;
                profile.UpdatedAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateNotificationSettingsAsync(int userId, bool push, bool email, bool sms)
        {
            var profile = await GetByUserIdAsync(userId);
            if (profile != null)
            {
                profile.PushEnabled = push;
                profile.EmailEnabled = email;
                profile.SmsEnabled = sms;
                profile.UpdatedAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}