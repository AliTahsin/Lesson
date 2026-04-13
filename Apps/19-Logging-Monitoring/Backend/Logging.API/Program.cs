using Serilog;
using Prometheus;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Logging.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom services
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<IMetricsService, MetricsService>();
builder.Services.AddScoped<ITracingService, TracingService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// OpenTelemetry Tracing
if (builder.Configuration.GetValue<bool>("Jaeger:Enabled"))
{
    builder.Services.AddOpenTelemetry()
        .WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddSource("Logging.API")
                .SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(builder.Configuration["Jaeger:ServiceName"] ?? "logging-api"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddJaegerExporter(options =>
                {
                    options.AgentHost = builder.Configuration["Jaeger:AgentHost"] ?? "localhost";
                    options.AgentPort = builder.Configuration.GetValue<int>("Jaeger:AgentPort", 6831);
                });
        });
}

// Prometheus Metrics
if (builder.Configuration.GetValue<bool>("Prometheus:Enabled"))
{
    builder.Services.AddHealthChecks();
}

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

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Prometheus metrics endpoint
if (builder.Configuration.GetValue<bool>("Prometheus:Enabled"))
{
    app.UseHttpMetrics();
    app.UseMetricServer();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Serilog request logging
app.UseSerilogRequestLogging();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();