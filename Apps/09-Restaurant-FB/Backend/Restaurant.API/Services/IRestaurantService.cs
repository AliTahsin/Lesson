using Restaurant.API.DTOs;

namespace Restaurant.API.Services
{
    public interface IRestaurantService
    {
        Task<RestaurantDto> GetRestaurantByIdAsync(int id);
        Task<List<RestaurantDto>> GetRestaurantsByHotelAsync(int hotelId);
        Task<RestaurantDto> CreateRestaurantAsync(CreateRestaurantDto dto);
        Task<RestaurantDto> UpdateRestaurantAsync(int id, UpdateRestaurantDto dto);
        Task<bool> DeleteRestaurantAsync(int id);
        Task<List<TableDto>> GetTablesByRestaurantAsync(int restaurantId);
        Task<TableDto> UpdateTableStatusAsync(int tableId, string status);
        Task<string> GenerateTableQrCodeAsync(int tableId);
    }
}