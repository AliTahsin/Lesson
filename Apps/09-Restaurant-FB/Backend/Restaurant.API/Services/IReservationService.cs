using Restaurant.API.DTOs;

namespace Restaurant.API.Services
{
    public interface IReservationService
    {
        Task<ReservationDto> GetReservationByIdAsync(int id);
        Task<List<ReservationDto>> GetReservationsByRestaurantAsync(int restaurantId);
        Task<List<ReservationDto>> GetReservationsByDateAsync(int restaurantId, DateTime date);
        Task<ReservationDto> CreateReservationAsync(CreateReservationDto dto);
        Task<ReservationDto> ConfirmReservationAsync(int id);
        Task<ReservationDto> CancelReservationAsync(int id, string reason);
        Task<List<TableDto>> GetAvailableTablesAsync(int restaurantId, DateTime date, string time, int guestCount);
    }
}