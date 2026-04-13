using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Auth.API.Models;
using Auth.API.Security;

namespace Auth.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly List<RefreshToken> _refreshTokens;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _refreshTokens = new List<RefreshToken>();
        }

        public string GenerateAccessToken(User user, List<string> permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim("userId", user.Id.ToString()),
                new Claim("hotelId", user.HotelId.ToString()),
                new Claim("department", user.Department ?? ""),
                new Claim("position", user.Position ?? "")
            };

            // Add role claims
            foreach (var roleId in user.RoleIds)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleId.ToString()));
            }

            // Add permission claims
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(int userId, string deviceId, string deviceName, string ipAddress, string userAgent)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            
            var refreshToken = new RefreshToken
            {
                Id = _refreshTokens.Count + 1,
                UserId = userId,
                Token = token,
                DeviceId = deviceId,
                DeviceName = deviceName,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                IsRevoked = false,
                CreatedAt = DateTime.Now
            };
            
            _refreshTokens.Add(refreshToken);
            return token;
        }

        public async Task<(int? UserId, RefreshToken Token)> ValidateRefreshTokenAsync(string token)
        {
            var refreshToken = _refreshTokens.FirstOrDefault(rt => rt.Token == token && !rt.IsRevoked);
            if (refreshToken == null || refreshToken.ExpiryDate < DateTime.Now)
                return (null, null);
            
            return await Task.FromResult((refreshToken.UserId, refreshToken));
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = _refreshTokens.FirstOrDefault(rt => rt.Token == token);
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}