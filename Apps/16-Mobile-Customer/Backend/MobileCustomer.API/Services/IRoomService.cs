using MobileCustomer.API.DTOs;

namespace MobileCustomer.API.Services
{
    public interface IRoomService
    {
        Task<List<MenuItemDto>> GetMenuAsync();
        Task<List<MenuItemDto>> GetMenuByCategoryAsync(string category);
        Task<List<string>> GetCategoriesAsync();
        Task<RoomServiceOrderDto> CreateOrderAsync(int userId, CreateRoomServiceOrderDto dto);
        Task<List<RoomServiceOrderDto>> GetUserOrdersAsync(int userId);
        Task<RoomServiceOrderDto> GetOrderByIdAsync(int orderId, int userId);
        Task<bool> CancelOrderAsync(int orderId, int userId);
        Task<OrderStatusDto> TrackOrderAsync(int orderId, int userId);
    }
}