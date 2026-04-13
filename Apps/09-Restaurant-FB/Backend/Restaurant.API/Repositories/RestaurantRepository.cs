using Restaurant.API.Models;
using Restaurant.API.Data;

namespace Restaurant.API.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly List<Restaurant> _restaurants;
        private readonly List<Table> _tables;

        public RestaurantRepository()
        {
            _restaurants = MockData.GetRestaurants();
            _tables = MockData.GetTables();
        }

        public async Task<Restaurant> GetByIdAsync(int id)
        {
            return await Task.FromResult(_restaurants.FirstOrDefault(r => r.Id == id));
        }

        public async Task<List<Restaurant>> GetAllAsync()
        {
            return await Task.FromResult(_restaurants.ToList());
        }

        public async Task<List<Restaurant>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_restaurants.Where(r => r.HotelId == hotelId).ToList());
        }

        public async Task<Restaurant> AddAsync(Restaurant restaurant)
        {
            restaurant.Id = _restaurants.Max(r => r.Id) + 1;
            restaurant.CreatedAt = DateTime.Now;
            _restaurants.Add(restaurant);
            return await Task.FromResult(restaurant);
        }

        public async Task<Restaurant> UpdateAsync(Restaurant restaurant)
        {
            var existing = await GetByIdAsync(restaurant.Id);
            if (existing != null)
            {
                var index = _restaurants.IndexOf(existing);
                restaurant.UpdatedAt = DateTime.Now;
                _restaurants[index] = restaurant;
            }
            return await Task.FromResult(restaurant);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var restaurant = await GetByIdAsync(id);
            if (restaurant != null)
            {
                _restaurants.Remove(restaurant);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<Table>> GetTablesByRestaurantAsync(int restaurantId)
        {
            return await Task.FromResult(_tables.Where(t => t.RestaurantId == restaurantId).ToList());
        }

        public async Task<Table> GetTableByIdAsync(int tableId)
        {
            return await Task.FromResult(_tables.FirstOrDefault(t => t.Id == tableId));
        }

        public async Task<Table> UpdateTableStatusAsync(int tableId, string status)
        {
            var table = await GetTableByIdAsync(tableId);
            if (table != null)
            {
                table.Status = status;
                var index = _tables.IndexOf(table);
                _tables[index] = table;
            }
            return await Task.FromResult(table);
        }
    }
}