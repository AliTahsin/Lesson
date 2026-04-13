using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.API.DTOs;
using Notification.API.Services;

namespace Notification.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PushController : ControllerBase
    {
        private readonly IPushService _pushService;

        public PushController(IPushService pushService)
        {
            _pushService = pushService;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] PushSubscriptionDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var result = await _pushService.SubscribeAsync(userId, dto.Endpoint, dto.P256dh, dto.Auth, dto.DeviceType);
            return Ok(new { success = result, message = result ? "Subscribed successfully" : "Subscription failed" });
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromBody] UnsubscribeDto dto)
        {
            var result = await _pushService.UnsubscribeAsync(dto.Endpoint);
            return Ok(new { success = result });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("send")]
        public async Task<IActionResult> SendPush([FromBody] PushSendDto dto)
        {
            bool result;
            
            if (dto.UserId.HasValue)
            {
                result = await _pushService.SendToUserAsync(dto.UserId.Value, dto.Title, dto.Body, dto.Data);
            }
            else if (!string.IsNullOrEmpty(dto.Role))
            {
                result = await _pushService.SendToRoleAsync(dto.Role, dto.Title, dto.Body, dto.Data);
            }
            else if (dto.HotelId.HasValue)
            {
                result = await _pushService.SendToHotelAsync(dto.HotelId.Value, dto.Title, dto.Body, dto.Data);
            }
            else
            {
                result = await _pushService.SendToAllAsync(dto.Title, dto.Body, dto.Data);
            }
            
            return Ok(new { success = result });
        }
    }

    public class UnsubscribeDto
    {
        public string Endpoint { get; set; }
    }
}