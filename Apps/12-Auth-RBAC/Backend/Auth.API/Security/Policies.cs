using Microsoft.AspNetCore.Authorization;

namespace Auth.API.Security
{
    public static class Policies
    {
        public const string AdminOnly = "AdminOnly";
        public const string HotelManager = "HotelManager";
        public const string FrontDeskStaff = "FrontDeskStaff";
        public const string HousekeepingStaff = "HousekeepingStaff";
        public const string CanManageUsers = "CanManageUsers";
        public const string CanViewReports = "CanViewReports";
        public const string CanManageReservations = "CanManageReservations";

        public static AuthorizationPolicy AdminOnlyPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole("1") // Admin role ID
                .Build();
        }

        public static AuthorizationPolicy HotelManagerPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                    context.User.HasClaim("permission", "hotel:manage") ||
                    context.User.HasClaim(ClaimTypes.Role, "1"))
                .Build();
        }

        public static AuthorizationPolicy PermissionPolicy(string permission)
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                    context.User.HasClaim("permission", permission) ||
                    context.User.HasClaim(ClaimTypes.Role, "1"))
                .Build();
        }
    }
}