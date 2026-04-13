using Auth.API.Models;

namespace Auth.API.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, List<string> permissions);
        string GenerateRefreshToken(int userId, string deviceId, string deviceName, string ipAddress, string userAgent);
        Task<(int? UserId, RefreshToken Token)> ValidateRefreshTokenAsync(string token);
        Task<bool> RevokeRefreshTokenAsync(string token);
    }
}