using ChannelManagement.API.Repositories;
using ChannelManagement.API.Services;
using ChannelManagement.API.Data;
using ChannelManagement.API.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories with mock data
builder.Services.AddSingleton<List<Channel>>(MockData.GetChannels());
builder.Services.AddSingleton<List<ChannelConnection>>(MockData.GetChannelConnections());
builder.Services.AddSingleton<List<ChannelBooking>>(MockData.GetChannelBookings());
builder.Services.AddSingleton<List<SyncLog>>(MockData.GetSyncLogs());

builder.Services.AddScoped<IChannelRepository, ChannelRepository>();
builder.Services.AddScoped<IChannelConnectionRepository, ChannelConnectionRepository>();
builder.Services.AddScoped<IChannelBookingRepository, ChannelBookingRepository>();
builder.Services.AddScoped<ISyncLogRepository, SyncLogRepository>();
builder.Services.AddScoped<ChannelSyncService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

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
app.UseAuthorization();
app.MapControllers();

app.Run();