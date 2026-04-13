using Chatbot.API.Models;

namespace Chatbot.API.Repositories
{
    public interface IChatRepository
    {
        // Conversation
        Task<Conversation> GetConversationByIdAsync(int id);
        Task<Conversation> GetActiveConversationByUserAsync(int userId);
        Task<Conversation> CreateConversationAsync(Conversation conversation);
        Task<Conversation> UpdateConversationAsync(Conversation conversation);
        Task<bool> EndConversationAsync(int id);
        
        // Messages
        Task<ChatMessage> AddMessageAsync(ChatMessage message);
        Task<List<ChatMessage>> GetMessagesByConversationAsync(int conversationId);
        Task<bool> MarkMessagesAsReadAsync(int conversationId, int userId);
        
        // Statistics
        Task<int> GetActiveConversationCountAsync();
        Task<int> GetTotalConversationCountAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}