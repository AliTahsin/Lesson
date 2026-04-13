using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chatbot.API.DTOs;
using Chatbot.API.Services;

namespace Chatbot.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ConversationController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserConversations(int userId)
        {
            var conversation = await _chatService.GetActiveConversationAsync(userId);
            return Ok(conversation);
        }

        [HttpGet("{id}/messages")]
        public async Task<IActionResult> GetMessages(int id)
        {
            var messages = await _chatService.GetConversationMessagesAsync(id);
            return Ok(messages);
        }

        [HttpPost("{id}/end")]
        public async Task<IActionResult> EndConversation(int id)
        {
            var result = await _chatService.EndConversationAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Conversation ended successfully" });
        }

        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var stats = await _chatService.GetChatStatisticsAsync(startDate, endDate);
            return Ok(stats);
        }
    }
}