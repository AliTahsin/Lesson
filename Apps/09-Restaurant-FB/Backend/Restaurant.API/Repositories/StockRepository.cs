using Restaurant.API.Models;
using Restaurant.API.Data;

namespace Restaurant.API.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly List<StockItem> _stockItems;

        public StockRepository()
        {
            _stockItems = MockData.GetStockItems();
        }

        public async Task<StockItem> GetByIdAsync(int id)
        {
            return await Task.FromResult(_stockItems.FirstOrDefault(s => s.Id == id));
        }

        public async Task<List<StockItem>> GetByRestaurantAsync(int restaurantId)
        {
            return await Task.FromResult(_stockItems.Where(s => s.RestaurantId == restaurantId).ToList());
        }

        public async Task<List<StockItem>> GetLowStockItemsAsync(int restaurantId)
        {
            return await Task.FromResult(_stockItems
                .Where(s => s.RestaurantId == restaurantId && s.CurrentStock <= s.ReorderLevel)
                .ToList());
        }

        public async Task<StockItem> AddAsync(StockItem item)
        {
            item.Id = _stockItems.Max(s => s.Id) + 1;
            item.CreatedAt = DateTime.Now;
            _stockItems.Add(item);
            return await Task.FromResult(item);
        }

        public async Task<StockItem> UpdateAsync(StockItem item)
        {
            var existing = await GetByIdAsync(item.Id);
            if (existing != null)
            {
                var index = _stockItems.IndexOf(existing);
                item.UpdatedAt = DateTime.Now;
                _stockItems[index] = item;
            }
            return await Task.FromResult(item);
        }

        public async Task<StockItem> UpdateStockAsync(int id, int quantity)
        {
            var item = await GetByIdAsync(id);
            if (item != null)
            {
                item.CurrentStock = quantity;
                item.UpdatedAt = DateTime.Now;
                if (quantity <= item.ReorderLevel)
                {
                    // Trigger reorder alert
                }
            }
            return await Task.FromResult(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await GetByIdAsync(id);
            if (item != null)
            {
                _stockItems.Remove(item);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<StockItem>> GetExpiringItemsAsync(int restaurantId, int days = 7)
        {
            var expiryDate = DateTime.Now.AddDays(days);
            return await Task.FromResult(_stockItems
                .Where(s => s.RestaurantId == restaurantId && 
                           s.ExpiryDate.HasValue && 
                           s.ExpiryDate.Value <= expiryDate)
                .ToList());
        }
    }
}