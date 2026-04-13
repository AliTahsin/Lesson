using Microsoft.AspNetCore.Mvc;
using Restaurant.API.DTOs;
using Restaurant.API.Services;

namespace Restaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
            if (restaurant == null) return NotFound();
            return Ok(restaurant);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var restaurants = await _restaurantService.GetRestaurantsByHotelAsync(hotelId);
            return Ok(restaurants);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantDto dto)
        {
            var restaurant = await _restaurantService.CreateRestaurantAsync(dto);
            return Ok(restaurant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRestaurantDto dto)
        {
            try
            {
                var restaurant = await _restaurantService.UpdateRestaurantAsync(id, dto);
                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _restaurantService.DeleteRestaurantAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Restaurant deleted successfully" });
        }

        [HttpGet("{id}/tables")]
        public async Task<IActionResult> GetTables(int id)
        {
            var tables = await _restaurantService.GetTablesByRestaurantAsync(id);
            return Ok(tables);
        }

        [HttpPost("tables/{tableId}/status")]
        public async Task<IActionResult> UpdateTableStatus(int tableId, [FromQuery] string status)
        {
            var table = await _restaurantService.UpdateTableStatusAsync(tableId, status);
            return Ok(table);
        }

        [HttpPost("tables/{tableId}/qrcode")]
        public async Task<IActionResult> GenerateQrCode(int tableId)
        {
            var qrUrl = await _restaurantService.GenerateTableQrCodeAsync(tableId);
            return Ok(new { qrUrl });
        }
    }
}