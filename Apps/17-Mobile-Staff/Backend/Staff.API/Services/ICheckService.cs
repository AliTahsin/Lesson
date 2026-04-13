using Staff.API.DTOs;

namespace Staff.API.Services
{
    public interface ICheckService
    {
        Task<CheckDto> CheckInAsync(CheckInDto dto, int staffId);
        Task<CheckDto> CheckOutAsync(CheckOutDto dto, int staffId);
        Task<List<CheckDto>> GetTodayCheckInsAsync(int hotelId);
        Task<List<CheckDto>> GetTodayCheckOutsAsync(int hotelId);
        Task<bool> CanCheckInAsync(int reservationId);
        Task<bool> CanCheckOutAsync(int reservationId);
    }
}