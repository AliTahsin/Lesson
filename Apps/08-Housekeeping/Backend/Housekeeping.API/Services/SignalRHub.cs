using Microsoft.AspNetCore.SignalR;

namespace Housekeeping.API.Services
{
    public class SignalRHub : Hub
    {
        private static readonly Dictionary<string, string> _connections = new();
        
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                _connections[Context.ConnectionId] = userId;
            }
            await base.OnConnectedAsync();
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _connections.Remove(Context.ConnectionId);
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
        
        public async Task SendTaskUpdate(int taskId, string status)
        {
            await Clients.All.SendAsync("TaskUpdated", taskId, status);
        }
        
        public async Task SendIssueUpdate(int issueId, string status)
        {
            await Clients.All.SendAsync("IssueUpdated", issueId, status);
        }
    }
}