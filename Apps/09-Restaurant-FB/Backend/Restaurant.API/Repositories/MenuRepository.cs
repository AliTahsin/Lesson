using Restaurant.API.Models;
using Restaurant.API.Data;

namespace Restaurant.API.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly List<Menu> _menus;
        private readonly List<MenuItem> _menuItems;

        public MenuRepository()
        {
            _menus = MockData.GetMenus();
            _menuItems = MockData.GetMenuItems();
        }

        public async Task<Menu> GetMenuByIdAsync(int id)
        {
            var menu = await Task.FromResult(_menus.FirstOrDefault(m => m.Id == id));
            if (menu != null)
            {
                menu.Items = _menuItems.Where(i => i.MenuId == id).ToList();
            }
            return menu;
        }

        public async Task<List<Menu>> GetMenusByRestaurantAsync(int restaurantId)
        {
            var menus = _menus.Where(m => m.RestaurantId == restaurantId).ToList();
            foreach (var menu in menus)
            {
                menu.Items = _menuItems.Where(i => i.MenuId == menu.Id).ToList();
            }
            return await Task.FromResult(menus);
        }

        public async Task<Menu> AddMenuAsync(Menu menu)
        {
            menu.Id = _menus.Max(m => m.Id) + 1;
            menu.CreatedAt = DateTime.Now;
            _menus.Add(menu);
            return await Task.FromResult(menu);
        }

        public async Task<Menu> UpdateMenuAsync(Menu menu)
        {
            var existing = await GetMenuByIdAsync(menu.Id);
            if (existing != null)
            {
                var index = _menus.IndexOf(existing);
                menu.UpdatedAt = DateTime.Now;
                _menus[index] = menu;
            }
            return await Task.FromResult(menu);
        }

        public async Task<bool> DeleteMenuAsync(int id)
        {
            var menu = await GetMenuByIdAsync(id);
            if (menu != null)
            {
                _menus.Remove(menu);
                var itemsToDelete = _menuItems.Where(i => i.MenuId == id).ToList();
                foreach (var item in itemsToDelete)
                {
                    _menuItems.Remove(item);
                }
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<MenuItem> GetMenuItemByIdAsync(int id)
        {
            return await Task.FromResult(_menuItems.FirstOrDefault(i => i.Id == id));
        }

        public async Task<List<MenuItem>> GetMenuItemsByMenuAsync(int menuId)
        {
            return await Task.FromResult(_menuItems.Where(i => i.MenuId == menuId).ToList());
        }

        public async Task<MenuItem> AddMenuItemAsync(MenuItem item)
        {
            item.Id = _menuItems.Max(i => i.Id) + 1;
            item.CreatedAt = DateTime.Now;
            _menuItems.Add(item);
            return await Task.FromResult(item);
        }

        public async Task<MenuItem> UpdateMenuItemAsync(MenuItem item)
        {
            var existing = await GetMenuItemByIdAsync(item.Id);
            if (existing != null)
            {
                var index = _menuItems.IndexOf(existing);
                item.UpdatedAt = DateTime.Now;
                _menuItems[index] = item;
            }
            return await Task.FromResult(item);
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            var item = await GetMenuItemByIdAsync(id);
            if (item != null)
            {
                _menuItems.Remove(item);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateMenuItemAvailabilityAsync(int id, bool isAvailable)
        {
            var item = await GetMenuItemByIdAsync(id);
            if (item != null)
            {
                item.IsAvailable = isAvailable;
                item.UpdatedAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}