using Microsoft.AspNetCore.Mvc;
using Restaurant.API.DTOs;
using Restaurant.API.Services;

namespace Restaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _stockService.GetStockItemByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetByRestaurant(int restaurantId)
        {
            var items = await _stockService.GetStockByRestaurantAsync(restaurantId);
            return Ok(items);
        }

        [HttpGet("restaurant/{restaurantId}/lowstock")]
        public async Task<IActionResult> GetLowStock(int restaurantId)
        {
            var items = await _stockService.GetLowStockItemsAsync(restaurantId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockItemDto dto)
        {
            var item = await _stockService.AddStockItemAsync(dto);
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStockItemDto dto)
        {
            try
            {
                var item = await _stockService.UpdateStockItemAsync(id, dto);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromQuery] int quantity)
        {
            var item = await _stockService.UpdateStockAsync(id, quantity);
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _stockService.DeleteStockItemAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Stock item deleted successfully" });
        }

        [HttpGet("restaurant/{restaurantId}/alerts")]
        public async Task<IActionResult> CheckAlerts(int restaurantId)
        {
            var alerts = await _stockService.CheckStockAlertsAsync(restaurantId);
            return Ok(alerts);
        }
    }
}