using Restaurant.API.Models;

namespace Restaurant.API.Repositories
{
    public interface IRestaurantRepository
    {
        Task<Restaurant> GetByIdAsync(int id);
        Task<List<Restaurant>> GetAllAsync();
        Task<List<Restaurant>> GetByHotelAsync(int hotelId);
        Task<Restaurant> AddAsync(Restaurant restaurant);
        Task<Restaurant> UpdateAsync(Restaurant restaurant);
        Task<bool> DeleteAsync(int id);
        Task<List<Table>> GetTablesByRestaurantAsync(int restaurantId);
        Task<Table> GetTableByIdAsync(int tableId);
        Task<Table> UpdateTableStatusAsync(int tableId, string status);
    }
}