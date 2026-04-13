using Microsoft.AspNetCore.Mvc;
using Restaurant.API.DTOs;
using Restaurant.API.Repositories;

namespace Restaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenusController : ControllerBase
    {
        private readonly IMenuRepository _menuRepository;

        public MenusController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetByRestaurant(int restaurantId)
        {
            var menus = await _menuRepository.GetMenusByRestaurantAsync(restaurantId);
            return Ok(menus);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(id);
            if (menu == null) return NotFound();
            return Ok(menu);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] Menu menu)
        {
            var created = await _menuRepository.AddMenuAsync(menu);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(int id, [FromBody] Menu menu)
        {
            menu.Id = id;
            var updated = await _menuRepository.UpdateMenuAsync(menu);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var result = await _menuRepository.DeleteMenuAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Menu deleted successfully" });
        }

        [HttpGet("items/{id}")]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            var item = await _menuRepository.GetMenuItemByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddMenuItem([FromBody] MenuItem item)
        {
            var created = await _menuRepository.AddMenuItemAsync(item);
            return Ok(created);
        }

        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItem item)
        {
            item.Id = id;
            var updated = await _menuRepository.UpdateMenuItemAsync(item);
            return Ok(updated);
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var result = await _menuRepository.DeleteMenuItemAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Menu item deleted successfully" });
        }

        [HttpPatch("items/{id}/availability")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromQuery] bool isAvailable)
        {
            var result = await _menuRepository.UpdateMenuItemAvailabilityAsync(id, isAvailable);
            if (!result) return NotFound();
            return Ok(new { message = $"Item availability updated to {isAvailable}" });
        }
    }
}