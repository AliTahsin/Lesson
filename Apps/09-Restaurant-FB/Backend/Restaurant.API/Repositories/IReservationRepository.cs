using Restaurant.API.Models;

namespace Restaurant.API.Repositories
{
    public interface IReservationRepository
    {
        Task<TableReservation> GetByIdAsync(int id);
        Task<TableReservation> GetByNumberAsync(string reservationNumber);
        Task<List<TableReservation>> GetByRestaurantAsync(int restaurantId);
        Task<List<TableReservation>> GetByDateAsync(int restaurantId, DateTime date);
        Task<List<TableReservation>> GetByCustomerAsync(int customerId);
        Task<TableReservation> CreateAsync(TableReservation reservation);
        Task<TableReservation> UpdateAsync(TableReservation reservation);
        Task<TableReservation> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);
        Task<List<Table>> GetAvailableTablesAsync(int restaurantId, DateTime date, string time, int guestCount);
    }
}