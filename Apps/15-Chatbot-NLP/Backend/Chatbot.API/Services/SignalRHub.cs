using Microsoft.AspNetCore.SignalR;

namespace Chatbot.API.Services
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

        public async Task JoinChat(int conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"chat-{conversationId}");
        }

        public async Task LeaveChat(int conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"chat-{conversationId}");
        }

        public async Task Typing(int conversationId, bool isTyping)
        {
            await Clients.Group($"chat-{conversationId}").SendAsync("UserTyping", isTyping);
        }
    }
}