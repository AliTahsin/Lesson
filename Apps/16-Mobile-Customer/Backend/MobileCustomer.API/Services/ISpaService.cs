using MobileCustomer.API.DTOs;

namespace MobileCustomer.API.Services
{
    public interface ISpaService
    {
        Task<List<SpaServiceDto>> GetServicesAsync();
        Task<SpaServiceDto> GetServiceByIdAsync(int id);
        Task<List<string>> GetAvailableTimesAsync(DateTime date, int serviceId);
        Task<SpaAppointmentDto> CreateAppointmentAsync(int userId, CreateSpaAppointmentDto dto);
        Task<List<SpaAppointmentDto>> GetUserAppointmentsAsync(int userId);
        Task<SpaAppointmentDto> GetAppointmentByIdAsync(int appointmentId, int userId);
        Task<bool> CancelAppointmentAsync(int appointmentId, int userId);
    }
}