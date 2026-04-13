using Microsoft.AspNetCore.SignalR;

namespace Restaurant.API.Services
{
    public class SignalRHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRestaurantGroup(int restaurantId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"restaurant-{restaurantId}");
        }

        public async Task JoinStockGroup(int restaurantId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"restaurant-{restaurantId}-stock");
        }

        public async Task JoinOrderGroup(int orderId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"order-{orderId}");
        }

        public async Task SendOrderUpdate(int orderId, string status)
        {
            await Clients.Group($"order-{orderId}").SendAsync("OrderUpdate", orderId, status);
        }
    }
}