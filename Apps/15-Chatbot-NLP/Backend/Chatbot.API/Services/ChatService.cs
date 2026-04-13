using AutoMapper;
using Chatbot.API.Models;
using Chatbot.API.DTOs;
using Chatbot.API.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Chatbot.API.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly INLPService _nlpService;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<ChatService> _logger;

        public ChatService(
            IChatRepository chatRepository,
            INLPService nlpService,
            IMapper mapper,
            IHubContext<SignalRHub> hubContext,
            ILogger<ChatService> logger)
        {
            _chatRepository = chatRepository;
            _nlpService = nlpService;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<ConversationDto> StartConversationAsync(int userId, string userName, string userEmail, int? hotelId)
        {
            // Check for existing active conversation
            var existing = await _chatRepository.GetActiveConversationByUserAsync(userId);
            if (existing != null)
            {
                return _mapper.Map<ConversationDto>(existing);
            }

            var conversation = new Conversation
            {
                UserId = userId,
                UserName = userName,
                UserEmail = userEmail,
                HotelId = hotelId,
                Status = "Active",
                IsBotActive = true,
                MessageCount = 0,
                StartedAt = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            var created = await _chatRepository.CreateConversationAsync(conversation);
            
            // Send welcome message
            var welcomeMessage = new ChatMessage
            {
                ConversationId = created.Id,
                SenderType = "Bot",
                SenderName = "Hotel Assistant",
                Message = "Merhaba! Size nasıl yardımcı olabilirim? Rezervasyon yapmak, oda bilgisi almak veya diğer sorularınız için buradayım.",
                Intent = "Welcome",
                Status = "Sent",
                SentAt = DateTime.Now
            };
            await _chatRepository.AddMessageAsync(welcomeMessage);
            
            await _hubContext.Clients.User(userId.ToString()).SendAsync("NewMessage", welcomeMessage);
            
            return _mapper.Map<ConversationDto>(created);
        }

        public async Task<ChatMessageDto> SendMessageAsync(int conversationId, string message, int? senderId, string senderName)
        {
            var userMessage = new ChatMessage
            {
                ConversationId = conversationId,
                SenderType = "User",
                SenderId = senderId,
                SenderName = senderName,
                Message = message,
                Status = "Sent",
                SentAt = DateTime.Now
            };
            
            await _chatRepository.AddMessageAsync(userMessage);
            
            // Send to SignalR
            if (senderId.HasValue)
            {
                await _hubContext.Clients.User(senderId.Value.ToString()).SendAsync("NewMessage", userMessage);
            }
            
            // Process with NLP and get bot response
            var nlpResult = await _nlpService.ProcessMessageAsync(message);
            
            var botMessage = new ChatMessage
            {
                ConversationId = conversationId,
                SenderType = "Bot",
                SenderName = "Hotel Assistant",
                Message = nlpResult.Response,
                Intent = nlpResult.Intent,
                Entities = JsonSerializer.Serialize(nlpResult.Entities),
                Status = "Sent",
                SentAt = DateTime.Now
            };
            
            await _chatRepository.AddMessageAsync(botMessage);
            
            if (senderId.HasValue)
            {
                await _hubContext.Clients.User(senderId.Value.ToString()).SendAsync("NewMessage", botMessage);
            }
            
            return _mapper.Map<ChatMessageDto>(userMessage);
        }

        public async Task<List<ChatMessageDto>> GetConversationMessagesAsync(int conversationId)
        {
            var messages = await _chatRepository.GetMessagesByConversationAsync(conversationId);
            return _mapper.Map<List<ChatMessageDto>>(messages);
        }

        public async Task<ConversationDto> GetActiveConversationAsync(int userId)
        {
            var conversation = await _chatRepository.GetActiveConversationByUserAsync(userId);
            return conversation != null ? _mapper.Map<ConversationDto>(conversation) : null;
        }

        public async Task<bool> EndConversationAsync(int conversationId)
        {
            return await _chatRepository.EndConversationAsync(conversationId);
        }

        public async Task<ChatStatisticsDto> GetChatStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var totalConversations = await _chatRepository.GetTotalConversationCountAsync(startDate, endDate);
            var activeConversations = await _chatRepository.GetActiveConversationCountAsync();
            
            return new ChatStatisticsDto
            {
                TotalConversations = totalConversations,
                ActiveConversations = activeConversations,
                ResolvedConversations = totalConversations - activeConversations,
                AverageResponseTime = 45, // Mock data
                SatisfactionRate = 92.5m // Mock data
            };
        }
    }
}