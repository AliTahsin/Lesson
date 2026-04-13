using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chatbot.API.DTOs;
using Chatbot.API.Services;

namespace Chatbot.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartConversation([FromQuery] int? hotelId)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var userName = User.FindFirst("name")?.Value ?? User.FindFirst("given_name")?.Value ?? "User";
            var userEmail = User.FindFirst("email")?.Value ?? "";
            
            var conversation = await _chatService.StartConversationAsync(userId, userName, userEmail, hotelId);
            return Ok(conversation);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveConversation()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var conversation = await _chatService.GetActiveConversationAsync(userId);
            return Ok(conversation);
        }

        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessages(int conversationId)
        {
            var messages = await _chatService.GetConversationMessagesAsync(conversationId);
            return Ok(messages);
        }

        [HttpPost("{conversationId}/send")]
        public async Task<IActionResult> SendMessage(int conversationId, [FromBody] SendMessageDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var userName = User.FindFirst("name")?.Value ?? User.FindFirst("given_name")?.Value ?? "User";
            
            var message = await _chatService.SendMessageAsync(conversationId, dto.Message, userId, userName);
            return Ok(message);
        }

        [HttpPost("{conversationId}/end")]
        public async Task<IActionResult> EndConversation(int conversationId)
        {
            var result = await _chatService.EndConversationAsync(conversationId);
            return Ok(new { success = result });
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var stats = await _chatService.GetChatStatisticsAsync(startDate, endDate);
            return Ok(stats);
        }
    }
}