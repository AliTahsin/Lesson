using MICE.API.DTOs;

namespace MICE.API.Services
{
    public interface IAttendeeService
    {
        Task<AttendeeDto> GetAttendeeByIdAsync(int id);
        Task<List<AttendeeDto>> GetAttendeesByEventAsync(int eventId);
        Task<AttendeeDto> RegisterAttendeeAsync(CreateAttendeeDto dto);
        Task<AttendeeDto> UpdateAttendeeAsync(int id, UpdateAttendeeDto dto);
        Task<AttendeeDto> CheckInAsync(int attendeeId);
        Task<AttendeeDto> CheckInByQrCodeAsync(string qrCode);
        Task<bool> DeleteAttendeeAsync(int id);
        Task<AttendeeStatisticsDto> GetAttendeeStatisticsAsync(int eventId);
    }
}