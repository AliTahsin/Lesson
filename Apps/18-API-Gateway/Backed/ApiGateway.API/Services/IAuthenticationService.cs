using System.Security.Claims;

namespace ApiGateway.API.Services
{
    public interface IAuthenticationService
    {
        Task<ClaimsPrincipal> ValidateTokenAsync(string token);
        bool IsAuthorized(ClaimsPrincipal user, string path, string method);
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);
    }
}