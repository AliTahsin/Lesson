using Chatbot.API.Models;
using Chatbot.API.Data;

namespace Chatbot.API.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly List<Conversation> _conversations;
        private readonly List<ChatMessage> _messages;

        public ChatRepository()
        {
            _conversations = MockData.GetConversations();
            _messages = MockData.GetMessages();
        }

        public async Task<Conversation> GetConversationByIdAsync(int id)
        {
            return await Task.FromResult(_conversations.FirstOrDefault(c => c.Id == id));
        }

        public async Task<Conversation> GetActiveConversationByUserAsync(int userId)
        {
            return await Task.FromResult(_conversations.FirstOrDefault(c => 
                c.UserId == userId && c.Status == "Active"));
        }

        public async Task<Conversation> CreateConversationAsync(Conversation conversation)
        {
            conversation.Id = _conversations.Max(c => c.Id) + 1;
            conversation.CreatedAt = DateTime.Now;
            conversation.StartedAt = DateTime.Now;
            _conversations.Add(conversation);
            return await Task.FromResult(conversation);
        }

        public async Task<Conversation> UpdateConversationAsync(Conversation conversation)
        {
            var existing = await GetConversationByIdAsync(conversation.Id);
            if (existing != null)
            {
                var index = _conversations.IndexOf(existing);
                conversation.UpdatedAt = DateTime.Now;
                _conversations[index] = conversation;
            }
            return await Task.FromResult(conversation);
        }

        public async Task<bool> EndConversationAsync(int id)
        {
            var conversation = await GetConversationByIdAsync(id);
            if (conversation != null)
            {
                conversation.Status = "Resolved";
                conversation.EndedAt = DateTime.Now;
                conversation.UpdatedAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<ChatMessage> AddMessageAsync(ChatMessage message)
        {
            message.Id = _messages.Max(m => m.Id) + 1;
            message.SentAt = DateTime.Now;
            _messages.Add(message);
            
            // Update conversation last message time
            var conversation = await GetConversationByIdAsync(message.ConversationId);
            if (conversation != null)
            {
                conversation.LastMessageAt = message.SentAt;
                conversation.MessageCount++;
                await UpdateConversationAsync(conversation);
            }
            
            return await Task.FromResult(message);
        }

        public async Task<List<ChatMessage>> GetMessagesByConversationAsync(int conversationId)
        {
            return await Task.FromResult(_messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt)
                .ToList());
        }

        public async Task<bool> MarkMessagesAsReadAsync(int conversationId, int userId)
        {
            var messages = _messages.Where(m => m.ConversationId == conversationId && m.SenderId != userId && m.ReadAt == null);
            foreach (var message in messages)
            {
                message.ReadAt = DateTime.Now;
                message.Status = "Read";
            }
            return await Task.FromResult(true);
        }

        public async Task<int> GetActiveConversationCountAsync()
        {
            return await Task.FromResult(_conversations.Count(c => c.Status == "Active"));
        }

        public async Task<int> GetTotalConversationCountAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _conversations.AsQueryable();
            if (startDate.HasValue)
                query = query.Where(c => c.CreatedAt >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(c => c.CreatedAt <= endDate.Value);
            return await Task.FromResult(query.Count());
        }
    }
}