using Auth.API.DTOs;

namespace Auth.API.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<LoginResponseDto> LoginWith2FAAsync(TwoFactorVerifyDto dto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<TokenDto> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(int userId, string refreshToken);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
        Task<bool> EnableTwoFactorAsync(int userId, string method);
        Task<bool> DisableTwoFactorAsync(int userId);
        Task<string> SendTwoFactorCodeAsync(int userId);
        Task<bool> VerifyTwoFactorCodeAsync(int userId, string code);
    }
}