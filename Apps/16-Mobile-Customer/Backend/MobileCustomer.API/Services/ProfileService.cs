using AutoMapper;
using MobileCustomer.API.Models;
using MobileCustomer.API.DTOs;
using MobileCustomer.API.Repositories;

namespace MobileCustomer.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(
            IProfileRepository profileRepository,
            IMapper mapper,
            ILogger<ProfileService> logger)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProfileDto> GetProfileAsync(int userId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                // Create default profile if not exists
                profile = new CustomerProfile
                {
                    UserId = userId,
                    Language = "tr",
                    BiometricEnabled = false,
                    PushEnabled = true,
                    EmailEnabled = true,
                    SmsEnabled = false,
                    CreatedAt = DateTime.Now
                };
                await _profileRepository.AddAsync(profile);
            }
            return _mapper.Map<ProfileDto>(profile);
        }

        public async Task<ProfileDto> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                profile = new CustomerProfile { UserId = userId };
                await _profileRepository.AddAsync(profile);
            }
            
            _mapper.Map(dto, profile);
            profile.UpdatedAt = DateTime.Now;
            
            await _profileRepository.UpdateAsync(profile);
            return _mapper.Map<ProfileDto>(profile);
        }

        public async Task<bool> UpdateBiometricAsync(int userId, bool enabled, string biometricKey)
        {
            return await _profileRepository.UpdateBiometricAsync(userId, enabled, biometricKey);
        }

        public async Task<bool> UpdateLanguageAsync(int userId, string language)
        {
            return await _profileRepository.UpdateLanguageAsync(userId, language);
        }

        public async Task<bool> UpdateNotificationSettingsAsync(int userId, NotificationSettingsDto dto)
        {
            return await _profileRepository.UpdateNotificationSettingsAsync(userId, dto.PushEnabled, dto.EmailEnabled, dto.SmsEnabled);
        }

        public async Task<DeviceInfoDto> RegisterDeviceAsync(int userId, RegisterDeviceDto dto)
        {
            // Simulate device registration
            var deviceInfo = new DeviceInfoDto
            {
                UserId = userId,
                DeviceId = dto.DeviceId,
                DeviceName = dto.DeviceName,
                Platform = dto.Platform,
                IsActive = true,
                LastActiveAt = DateTime.Now
            };
            return await Task.FromResult(deviceInfo);
        }

        public async Task<bool> UnregisterDeviceAsync(int userId, string deviceId)
        {
            return await Task.FromResult(true);
        }
    }
}