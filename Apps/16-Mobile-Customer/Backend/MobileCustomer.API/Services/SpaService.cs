using AutoMapper;
using MobileCustomer.API.Models;
using MobileCustomer.API.DTOs;

namespace MobileCustomer.API.Services
{
    public class SpaService : ISpaService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SpaService> _logger;

        public SpaService(IMapper mapper, ILogger<SpaService> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<SpaServiceDto>> GetServicesAsync()
        {
            var services = new List<SpaServiceDto>
            {
                new SpaServiceDto { Id = 1, Name = "Swedish Massage", Description = "Relaxing full body massage", DurationMinutes = 60, Price = 80, Category = "Massage" },
                new SpaServiceDto { Id = 2, Name = "Deep Tissue Massage", Description = "Intense muscle relief", DurationMinutes = 60, Price = 90, Category = "Massage" },
                new SpaServiceDto { Id = 3, Name = "Hot Stone Massage", Description = "Heated stone therapy", DurationMinutes = 75, Price = 110, Category = "Massage" },
                new SpaServiceDto { Id = 4, Name = "Facial Treatment", Description = "Deep cleansing facial", DurationMinutes = 45, Price = 70, Category = "Facial" },
                new SpaServiceDto { Id = 5, Name = "Anti-Aging Facial", Description = "Advanced age-defying treatment", DurationMinutes = 60, Price = 100, Category = "Facial" },
                new SpaServiceDto { Id = 6, Name = "Body Scrub", Description = "Exfoliating body treatment", DurationMinutes = 45, Price = 65, Category = "Body" },
                new SpaServiceDto { Id = 7, Name = "Body Wrap", Description = "Hydrating body wrap", DurationMinutes = 60, Price = 85, Category = "Body" },
                new SpaServiceDto { Id = 8, Name = "Sauna Session", Description = "Traditional Finnish sauna", DurationMinutes = 30, Price = 30, Category = "Wellness" },
                new SpaServiceDto { Id = 9, Name = "Steam Room", Description = "Aromatic steam therapy", DurationMinutes = 30, Price = 30, Category = "Wellness" },
                new SpaServiceDto { Id = 10, Name = "Manicure & Pedicure", Description = "Nail care treatment", DurationMinutes = 60, Price = 50, Category = "Nail" }
            };
            
            return await Task.FromResult(services);
        }

        public async Task<SpaServiceDto> GetServiceByIdAsync(int id)
        {
            var services = await GetServicesAsync();
            return services.FirstOrDefault(s => s.Id == id);
        }

        public async Task<List<string>> GetAvailableTimesAsync(DateTime date, int serviceId)
        {
            var times = new List<string>();
            for (int hour = 9; hour <= 20; hour++)
            {
                times.Add($"{hour:D2}:00");
                if (hour != 20) times.Add($"{hour:D2}:30");
            }
            
            return await Task.FromResult(times);
        }

        public async Task<SpaAppointmentDto> CreateAppointmentAsync(int userId, CreateSpaAppointmentDto dto)
        {
            var random = new Random();
            var service = await GetServiceByIdAsync(dto.ServiceId);
            
            var appointment = new SpaAppointmentDto
            {
                Id = random.Next(100, 1000),
                AppointmentNumber = $"SPA-{DateTime.Now.Ticks}",
                UserId = userId,
                ServiceName = service?.Name ?? "Spa Service",
                ServiceType = service?.Category ?? "Wellness",
                DurationMinutes = service?.DurationMinutes ?? 60,
                Price = service?.Price ?? 50,
                AppointmentDate = dto.AppointmentDate,
                AppointmentTime = dto.AppointmentTime,
                SpecialRequests = dto.SpecialRequests,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };
            
            return await Task.FromResult(appointment);
        }

        public async Task<List<SpaAppointmentDto>> GetUserAppointmentsAsync(int userId)
        {
            var appointments = new List<SpaAppointmentDto>();
            var random = new Random();
            
            for (int i = 1; i <= 3; i++)
            {
                appointments.Add(new SpaAppointmentDto
                {
                    Id = i,
                    AppointmentNumber = $"SPA-{DateTime.Now.AddDays(-i).Ticks}",
                    UserId = userId,
                    ServiceName = i == 1 ? "Swedish Massage" : (i == 2 ? "Facial Treatment" : "Body Scrub"),
                    AppointmentDate = DateTime.Now.AddDays(i),
                    AppointmentTime = $"{9 + i}:00",
                    Status = i == 1 ? "Confirmed" : "Pending",
                    Price = random.Next(50, 120),
                    CreatedAt = DateTime.Now.AddDays(-i)
                });
            }
            
            return await Task.FromResult(appointments);
        }

        public async Task<SpaAppointmentDto> GetAppointmentByIdAsync(int appointmentId, int userId)
        {
            var appointments = await GetUserAppointmentsAsync(userId);
            return appointments.FirstOrDefault(a => a.Id == appointmentId);
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId, int userId)
        {
            return await Task.FromResult(true);
        }
    }
}