using Microsoft.AspNetCore.SignalR;

namespace Staff.API.Services
{
    public class SignalRHub : Hub
    {
        private static readonly Dictionary<string, string> _userConnections = new();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections[Context.ConnectionId] = userId;
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _userConnections.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinHotelGroup(int hotelId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"hotel-{hotelId}");
        }

        public async Task JoinMaintenanceGroup(int hotelId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"hotel-{hotelId}-maintenance");
        }

        public async Task JoinStaffGroup(int staffId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"staff-{staffId}");
        }
    }
}