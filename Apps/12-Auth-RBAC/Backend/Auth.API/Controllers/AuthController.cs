using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Auth.API.DTOs;
using Auth.API.Services;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("login/2fa")]
        public async Task<IActionResult> LoginWith2FA([FromBody] TwoFactorVerifyDto dto)
        {
            try
            {
                var result = await _authService.LoginWith2FAAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var user = await _authService.RegisterAsync(registerDto);
                return Ok(new { message = "Registration successful", user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            try
            {
                var tokens = await _authService.RefreshTokenAsync(dto.RefreshToken);
                return Ok(tokens);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            await _authService.LogoutAsync(userId, dto.RefreshToken);
            return Ok(new { message = "Logout successful" });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            try
            {
                await _authService.ChangePasswordAsync(userId, dto);
                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            await _authService.ForgotPasswordAsync(dto.Email);
            return Ok(new { message = "If the email exists, a reset link has been sent" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                await _authService.ResetPasswordAsync(dto);
                return Ok(new { message = "Password reset successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("2fa/enable")]
        public async Task<IActionResult> EnableTwoFactor([FromBody] TwoFactorEnableDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            if (dto.Enabled)
            {
                await _authService.EnableTwoFactorAsync(userId, dto.Method);
            }
            else
            {
                await _authService.DisableTwoFactorAsync(userId);
            }
            return Ok(new { message = $"2FA {(dto.Enabled ? "enabled" : "disabled")} successfully" });
        }

        [Authorize]
        [HttpPost("2fa/send-code")]
        public async Task<IActionResult> SendTwoFactorCode()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            await _authService.SendTwoFactorCodeAsync(userId);
            return Ok(new { message = "Verification code sent" });
        }

        [Authorize]
        [HttpPost("2fa/verify")]
        public async Task<IActionResult> VerifyTwoFactorCode([FromBody] TwoFactorVerifyCodeDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var isValid = await _authService.VerifyTwoFactorCodeAsync(userId, dto.Code);
            if (!isValid)
                return BadRequest(new { error = "Invalid verification code" });
            return Ok(new { message = "Code verified successfully" });
        }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }

    public class TwoFactorVerifyCodeDto
    {
        public string Code { get; set; }
    }
}