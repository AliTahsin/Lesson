using Chatbot.API.DTOs;

namespace Chatbot.API.Services
{
    public interface IChatService
    {
        Task<ConversationDto> StartConversationAsync(int userId, string userName, string userEmail, int? hotelId);
        Task<ChatMessageDto> SendMessageAsync(int conversationId, string message, int? senderId, string senderName);
        Task<List<ChatMessageDto>> GetConversationMessagesAsync(int conversationId);
        Task<ConversationDto> GetActiveConversationAsync(int userId);
        Task<bool> EndConversationAsync(int conversationId);
        Task<ChatStatisticsDto> GetChatStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}