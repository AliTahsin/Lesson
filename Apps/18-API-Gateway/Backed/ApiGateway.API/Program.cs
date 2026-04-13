using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Yarp.ReverseProxy.Configuration;
using ApiGateway.API.Middlewares;
using ApiGateway.API.Services;
using ApiGateway.API.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {CorrelationId} {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/gateway-.txt", 
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {CorrelationId} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Gateway",
        Version = "v1",
        Description = "API Gateway for Hotel Management System"
    });
    
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token"
    });
    
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "your-super-secret-key-with-at-least-32-characters";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "HotelAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "HotelClient";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
        
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Log.Warning("Authentication failed: {Error}", context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Admin", "Manager"));
    options.AddPolicy("StaffOnly", policy => policy.RequireRole("Admin", "Manager", "Staff"));
});
// Health checks kısmını güncelle
builder.Services.AddHealthChecks()
    .AddCheck<ServiceHealthCheck>("gateway", tags: new[] { "ready", "live" });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
    
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://hotel.com", "https://admin.hotel.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// HTTP Client
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("ServiceClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Custom services
builder.Services.AddSingleton<IServiceDiscovery, ServiceDiscovery>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

// Reverse Proxy Configuration
var proxyConfig = builder.Configuration.GetSection("ReverseProxy");
builder.Services.AddReverseProxy()
    .LoadFromConfig(proxyConfig)
    .AddServiceDiscovery();

// Health checks
builder.Services.AddHealthChecks()
    .AddCheck<ServiceHealthCheck>("gateway");

// Memory Cache for rate limiting
builder.Services.AddMemoryCache();

// Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
});

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
    });
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Response Compression
app.UseResponseCompression();

// Middlewares order is important
app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "Production");

// Custom middlewares
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapHealthChecks("/health");
app.MapReverseProxy();

// Error handling endpoint
app.Map("/error", (HttpContext context) =>
{
    return Results.Problem("An error occurred while processing your request.");
});

// Register services from configuration
var services = builder.Configuration.GetSection("Services").GetChildren();
var serviceDiscovery = app.Services.GetRequiredService<IServiceDiscovery>();

foreach (var service in services)
{
    var serviceName = service.Key;
    var serviceUrl = service.Value;
    
    try
    {
        var uri = new Uri(serviceUrl);
        await serviceDiscovery.RegisterServiceAsync(new Service
        {
            Name = serviceName,
            Url = serviceUrl,
            Port = uri.Port,
            IsHealthy = true,
            RegisteredAt = DateTime.Now,
            LastHeartbeat = DateTime.Now
        });
        
        Log.Information("Service {ServiceName} registered at {ServiceUrl}", serviceName, serviceUrl);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Failed to register service {ServiceName}", serviceName);
    }
}

// Start health check background service
_ = Task.Run(async () =>
{
    while (true)
    {
        await Task.Delay(30000); // Check every 30 seconds
        await serviceDiscovery.CheckAllHealthAsync();
    }
});

Log.Information("API Gateway started successfully on {Urls}", string.Join(", ", app.Urls));

app.Run();