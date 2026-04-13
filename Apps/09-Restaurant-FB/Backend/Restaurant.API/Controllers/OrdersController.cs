using Microsoft.AspNetCore.Mvc;
using Restaurant.API.DTOs;
using Restaurant.API.Services;

namespace Restaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("number/{orderNumber}")]
        public async Task<IActionResult> GetByNumber(string orderNumber)
        {
            var order = await _orderService.GetOrderByNumberAsync(orderNumber);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetByRestaurant(int restaurantId)
        {
            var orders = await _orderService.GetOrdersByRestaurantAsync(restaurantId);
            return Ok(orders);
        }

        [HttpGet("restaurant/{restaurantId}/pending")]
        public async Task<IActionResult> GetPendingOrders(int restaurantId)
        {
            var orders = await _orderService.GetPendingOrdersAsync(restaurantId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(dto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, status);
            return Ok(order);
        }

        [HttpPatch("items/{orderItemId}/status")]
        public async Task<IActionResult> UpdateItemStatus(int orderItemId, [FromQuery] string status)
        {
            var order = await _orderService.UpdateOrderItemStatusAsync(orderItemId, status);
            return Ok(order);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id, [FromQuery] string reason)
        {
            var order = await _orderService.CancelOrderAsync(id, reason ?? "User requested");
            return Ok(order);
        }

        [HttpGet("restaurant/{restaurantId}/revenue")]
        public async Task<IActionResult> GetDailyRevenue(int restaurantId, [FromQuery] DateTime date)
        {
            var revenue = await _orderService.GetDailyRevenueAsync(restaurantId, date);
            return Ok(revenue);
        }

        [HttpGet("restaurant/{restaurantId}/popular")]
        public async Task<IActionResult> GetPopularItems(int restaurantId, [FromQuery] int days = 7)
        {
            var popular = await _orderService.GetPopularItemsAsync(restaurantId, days);
            return Ok(popular);
        }
    }
}