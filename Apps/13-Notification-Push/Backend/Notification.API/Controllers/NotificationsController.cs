using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.API.DTOs;
using Notification.API.Services;

namespace Notification.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new { count });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return Ok(notification);
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var result = await _notificationService.MarkAsReadAsync(id, userId);
            if (!result) return NotFound();
            return Ok(new { message = "Notification marked as read" });
        }

        [HttpPost("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new { message = "All notifications marked as read" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var result = await _notificationService.DeleteNotificationAsync(id, userId);
            if (!result) return NotFound();
            return Ok(new { message = "Notification deleted" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Send([FromBody] CreateNotificationDto dto)
        {
            var senderId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var senderName = User.FindFirst("name")?.Value ?? "System";
            
            dto.SenderId = senderId;
            dto.SenderName = senderName;
            
            var notification = await _notificationService.SendNotificationAsync(dto);
            return Ok(notification);
        }
    }
}