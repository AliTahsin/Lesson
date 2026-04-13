using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileCustomer.API.DTOs;
using MobileCustomer.API.Services;

namespace MobileCustomer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var profile = await _profileService.GetProfileAsync(userId);
            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var profile = await _profileService.UpdateProfileAsync(userId, dto);
            return Ok(profile);
        }

        [HttpPut("biometric")]
        public async Task<IActionResult> UpdateBiometric([FromBody] UpdateBiometricDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var result = await _profileService.UpdateBiometricAsync(userId, dto.Enabled, dto.BiometricKey);
            return Ok(new { success = result });
        }

        [HttpPut("language")]
        public async Task<IActionResult> UpdateLanguage([FromBody] UpdateLanguageDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var result = await _profileService.UpdateLanguageAsync(userId, dto.Language);
            return Ok(new { success = result });
        }

        [HttpPut("notifications")]
        public async Task<IActionResult> UpdateNotifications([FromBody] NotificationSettingsDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var result = await _profileService.UpdateNotificationSettingsAsync(userId, dto);
            return Ok(new { success = result });
        }

        [HttpPost("device/register")]
        public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var device = await _profileService.RegisterDeviceAsync(userId, dto);
            return Ok(device);
        }

        [HttpDelete("device/{deviceId}")]
        public async Task<IActionResult> UnregisterDevice(string deviceId)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var result = await _profileService.UnregisterDeviceAsync(userId, deviceId);
            return Ok(new { success = result });
        }
    }

    public class UpdateBiometricDto
    {
        public bool Enabled { get; set; }
        public string BiometricKey { get; set; }
    }

    public class UpdateLanguageDto
    {
        public string Language { get; set; }
    }
}