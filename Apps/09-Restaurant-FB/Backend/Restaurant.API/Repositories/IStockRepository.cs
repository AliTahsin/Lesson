using Restaurant.API.Models;

namespace Restaurant.API.Repositories
{
    public interface IStockRepository
    {
        Task<StockItem> GetByIdAsync(int id);
        Task<List<StockItem>> GetByRestaurantAsync(int restaurantId);
        Task<List<StockItem>> GetLowStockItemsAsync(int restaurantId);
        Task<StockItem> AddAsync(StockItem item);
        Task<StockItem> UpdateAsync(StockItem item);
        Task<StockItem> UpdateStockAsync(int id, int quantity);
        Task<bool> DeleteAsync(int id);
        Task<List<StockItem>> GetExpiringItemsAsync(int restaurantId, int days = 7);
    }
}