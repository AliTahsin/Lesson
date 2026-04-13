using Restaurant.API.Models;

namespace Restaurant.API.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> GetOrderByNumberAsync(string orderNumber);
        Task<List<Order>> GetOrdersByRestaurantAsync(int restaurantId);
        Task<List<Order>> GetOrdersByTableAsync(int tableId);
        Task<List<Order>> GetOrdersByCustomerAsync(int customerId);
        Task<List<Order>> GetOrdersByRoomAsync(int roomId);
        Task<List<Order>> GetOrdersByStatusAsync(string status);
        Task<List<Order>> GetPendingOrdersAsync(int restaurantId);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<Order> UpdateOrderStatusAsync(int orderId, string status);
        Task<OrderItem> UpdateOrderItemStatusAsync(int orderItemId, string status);
        Task<decimal> GetDailyRevenueAsync(int restaurantId, DateTime date);
        Task<Dictionary<string, int>> GetPopularItemsAsync(int restaurantId, int days = 7);
    }
}