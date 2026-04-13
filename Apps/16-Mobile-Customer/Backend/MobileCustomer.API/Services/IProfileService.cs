using MobileCustomer.API.DTOs;

namespace MobileCustomer.API.Services
{
    public interface IProfileService
    {
        Task<ProfileDto> GetProfileAsync(int userId);
        Task<ProfileDto> UpdateProfileAsync(int userId, UpdateProfileDto dto);
        Task<bool> UpdateBiometricAsync(int userId, bool enabled, string biometricKey);
        Task<bool> UpdateLanguageAsync(int userId, string language);
        Task<bool> UpdateNotificationSettingsAsync(int userId, NotificationSettingsDto dto);
        Task<DeviceInfoDto> RegisterDeviceAsync(int userId, RegisterDeviceDto dto);
        Task<bool> UnregisterDeviceAsync(int userId, string deviceId);
    }
}