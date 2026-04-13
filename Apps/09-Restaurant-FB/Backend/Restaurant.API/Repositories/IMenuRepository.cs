using Restaurant.API.Models;

namespace Restaurant.API.Repositories
{
    public interface IMenuRepository
    {
        Task<Menu> GetMenuByIdAsync(int id);
        Task<List<Menu>> GetMenusByRestaurantAsync(int restaurantId);
        Task<Menu> AddMenuAsync(Menu menu);
        Task<Menu> UpdateMenuAsync(Menu menu);
        Task<bool> DeleteMenuAsync(int id);
        Task<MenuItem> GetMenuItemByIdAsync(int id);
        Task<List<MenuItem>> GetMenuItemsByMenuAsync(int menuId);
        Task<MenuItem> AddMenuItemAsync(MenuItem item);
        Task<MenuItem> UpdateMenuItemAsync(MenuItem item);
        Task<bool> DeleteMenuItemAsync(int id);
        Task<bool> UpdateMenuItemAvailabilityAsync(int id, bool isAvailable);
    }
}