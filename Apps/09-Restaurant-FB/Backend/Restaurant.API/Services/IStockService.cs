using Restaurant.API.DTOs;

namespace Restaurant.API.Services
{
    public interface IStockService
    {
        Task<StockItemDto> GetStockItemByIdAsync(int id);
        Task<List<StockItemDto>> GetStockByRestaurantAsync(int restaurantId);
        Task<List<StockItemDto>> GetLowStockItemsAsync(int restaurantId);
        Task<StockItemDto> AddStockItemAsync(CreateStockItemDto dto);
        Task<StockItemDto> UpdateStockItemAsync(int id, UpdateStockItemDto dto);
        Task<StockItemDto> UpdateStockAsync(int id, int quantity);
        Task<bool> DeleteStockItemAsync(int id);
        Task<StockAlertDto> CheckStockAlertsAsync(int restaurantId);
    }
}