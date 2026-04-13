using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationService> _logger;

        // Route authorization rules
        private readonly Dictionary<string, (string[] Roles, string[] Permissions, bool RequireAuth)> _routeRules = new()
        {
            // Auth endpoints (public)
            { "/api/auth/login", (new[] { "*" }, new[] { "*" }, false) },
            { "/api/auth/register", (new[] { "*" }, new[] { "*" }, false) },
            { "/api/auth/refresh", (new[] { "*" }, new[] { "*" }, false) },
            { "/api/auth/forgot-password", (new[] { "*" }, new[] { "*" }, false) },
            { "/api/auth/reset-password", (new[] { "*" }, new[] { "*" }, false) },
            
            // Health endpoints (public)
            { "/health", (new[] { "*" }, new[] { "*" }, false) },
            
            // Admin only endpoints
            { "/api/users", (new[] { "Admin" }, new[] { "user:manage" }, true) },
            { "/api/roles", (new[] { "Admin" }, new[] { "role:manage" }, true) },
            { "/api/reports", (new[] { "Admin", "Manager" }, new[] { "report:view" }, true) },
            
            // Hotel endpoints
            { "/api/hotels", (new[] { "*" }, new[] { "*" }, false) },
            { "/api/hotels/{id}", (new[] { "*" }, new[] { "*" }, false) },
            { "/api/hotels/*/manage", (new[] { "Admin", "Manager" }, new[] { "hotel:manage" }, true) },
            
            // Reservation endpoints
            { "/api/reservations", (new[] { "*" }, new[] { "reservation:view" }, true) },
            { "/api/reservations/create", (new[] { "*" }, new[] { "reservation:create" }, true) },
            { "/api/reservations/*/cancel", (new[] { "*" }, new[] { "reservation:cancel" }, true) },
            
            // Payment endpoints
            { "/api/payments", (new[] { "*" }, new[] { "payment:view" }, true) },
            { "/api/payments/process", (new[] { "*" }, new[] { "payment:create" }, true) },
            
            // Staff endpoints
            { "/api/staff/*", (new[] { "Admin", "Manager" }, new[] { "staff:manage" }, true) },
            { "/api/tasks", (new[] { "Admin", "Housekeeping" }, new[] { "task:view" }, true) },
            { "/api/issues", (new[] { "Admin", "Maintenance" }, new[] { "issue:view" }, true) },
            
            // Default rule
            { "default", (new[] { "*" }, new[] { "*" }, true) }
        };

        public AuthenticationService(IConfiguration configuration, ILogger<AuthenticationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "your-super-secret-key-with-at-least-32-characters");
                
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"] ?? "HotelAPI",
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"] ?? "HotelClient",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return await Task.FromResult(principal);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token validation failed");
                return null;
            }
        }

        public bool IsAuthorized(ClaimsPrincipal user, string path, string method)
        {
            // Find matching rule
            var rule = FindMatchingRule(path);
            if (rule == null)
            {
                rule = _routeRules["default"];
            }
            
            // Check if authentication is required
            if (rule.RequireAuth && user == null)
            {
                return false;
            }
            
            // If no authentication required, allow access
            if (!rule.RequireAuth)
            {
                return true;
            }
            
            // Check roles
            if (rule.Roles.Length > 0 && rule.Roles[0] != "*")
            {
                var hasRole = rule.Roles.Any(role => user.HasClaim(ClaimTypes.Role, role) || user.HasClaim("role", role));
                if (!hasRole)
                {
                    return false;
                }
            }
            
            // Check permissions
            if (rule.Permissions.Length > 0 && rule.Permissions[0] != "*")
            {
                var hasPermission = rule.Permissions.Any(permission => user.HasClaim("permission", permission));
                if (!hasPermission)
                {
                    return false;
                }
            }
            
            return true;
        }

        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            if (user == null) return false;
            return await Task.FromResult(user.HasClaim("permission", permission));
        }

        private (string[] Roles, string[] Permissions, bool RequireAuth) FindMatchingRule(string path)
        {
            // Exact match
            if (_routeRules.ContainsKey(path))
            {
                return _routeRules[path];
            }
            
            // Pattern match (e.g., /api/hotels/*)
            foreach (var rule in _routeRules)
            {
                if (rule.Key.Contains("*"))
                {
                    var pattern = rule.Key.Replace("*", ".*");
                    if (System.Text.RegularExpressions.Regex.IsMatch(path, pattern))
                    {
                        return rule.Value;
                    }
                }
            }
            
            return null;
        }
    }
}