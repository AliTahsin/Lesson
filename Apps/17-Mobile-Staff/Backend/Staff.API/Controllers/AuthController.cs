using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Staff.API.DTOs;
using Staff.API.Services;

namespace Staff.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public AuthController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _staffService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var staffId = int.Parse(User.FindFirst("staffId")?.Value ?? "0");
            await _staffService.LogoutAsync(staffId);
            return Ok(new { message = "Logout successful" });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var staffId = int.Parse(User.FindFirst("staffId")?.Value ?? "0");
            var profile = await _staffService.GetStaffByIdAsync(staffId);
            return Ok(profile);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateStaffDto dto)
        {
            var staffId = int.Parse(User.FindFirst("staffId")?.Value ?? "0");
            var profile = await _staffService.UpdateProfileAsync(staffId, dto);
            return Ok(profile);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var staffId = int.Parse(User.FindFirst("staffId")?.Value ?? "0");
            await _staffService.ChangePasswordAsync(staffId, dto);
            return Ok(new { message = "Password changed successfully" });
        }
    }
}