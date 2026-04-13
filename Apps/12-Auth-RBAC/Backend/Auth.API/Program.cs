using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Auth.API.Repositories;
using Auth.API.Services;
using Auth.API.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Settings
var jwtSettings = new JwtSettings
{
    SecretKey = "your-super-secret-key-with-at-least-32-characters-long",
    Issuer = "HotelAPI",
    Audience = "HotelClient",
    AccessTokenExpirationMinutes = 15,
    RefreshTokenExpirationDays = 7
};
builder.Services.AddSingleton(jwtSettings);

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.AdminOnly, Policies.AdminOnlyPolicy());
    options.AddPolicy(Policies.HotelManager, Policies.HotelManagerPolicy());
    options.AddPolicy(Policies.CanManageUsers, policy => 
        policy.RequireAssertion(context =>
            context.User.HasClaim("permission", "user:manage") ||
            context.User.HasClaim(ClaimTypes.Role, "1")));
    options.AddPolicy(Policies.CanViewReports, policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim("permission", "report:view") ||
            context.User.HasClaim(ClaimTypes.Role, "1")));
    options.AddPolicy(Policies.CanManageReservations, policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim("permission", "reservation:manage") ||
            context.User.HasClaim(ClaimTypes.Role, "1")));
});

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITwoFactorService, TwoFactorService>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();