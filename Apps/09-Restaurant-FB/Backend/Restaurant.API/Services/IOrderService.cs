using Restaurant.API.DTOs;

namespace Restaurant.API.Services
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<OrderDto> GetOrderByNumberAsync(string orderNumber);
        Task<List<OrderDto>> GetOrdersByRestaurantAsync(int restaurantId);
        Task<List<OrderDto>> GetPendingOrdersAsync(int restaurantId);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, string status);
        Task<OrderDto> UpdateOrderItemStatusAsync(int orderItemId, string status);
        Task<OrderDto> CancelOrderAsync(int orderId, string reason);
        Task<DailyRevenueDto> GetDailyRevenueAsync(int restaurantId, DateTime date);
        Task<PopularItemsDto> GetPopularItemsAsync(int restaurantId, int days = 7);
    }
}