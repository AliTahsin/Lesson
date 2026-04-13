using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileCustomer.API.DTOs;
using MobileCustomer.API.Services;

namespace MobileCustomer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoomServiceController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomServiceController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("menu")]
        public async Task<IActionResult> GetMenu()
        {
            var menu = await _roomService.GetMenuAsync();
            return Ok(menu);
        }

        [HttpGet("menu/{category}")]
        public async Task<IActionResult> GetMenuByCategory(string category)
        {
            var menu = await _roomService.GetMenuByCategoryAsync(category);
            return Ok(menu);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _roomService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost("order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateRoomServiceOrderDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var order = await _roomService.CreateOrderAsync(userId, dto);
            return Ok(order);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var orders = await _roomService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet("orders/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var order = await _roomService.GetOrderByIdAsync(orderId, userId);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost("orders/{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var result = await _roomService.CancelOrderAsync(orderId, userId);
            return Ok(new { success = result });
        }

        [HttpGet("orders/track/{orderId}")]
        public async Task<IActionResult> TrackOrder(int orderId)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var status = await _roomService.TrackOrderAsync(orderId, userId);
            return Ok(status);
        }
    }
}