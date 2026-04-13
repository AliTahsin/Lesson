using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileCustomer.API.DTOs;
using MobileCustomer.API.Services;

namespace MobileCustomer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DigitalKeyController : ControllerBase
    {
        private readonly IDigitalKeyService _digitalKeyService;

        public DigitalKeyController(IDigitalKeyService digitalKeyService)
        {
            _digitalKeyService = digitalKeyService;
        }

        [HttpPost("generate/{reservationId}")]
        public async Task<IActionResult> GenerateKey(int reservationId)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var key = await _digitalKeyService.GenerateKeyAsync(reservationId, userId);
            return Ok(key);
        }

        [HttpGet("reservation/{reservationId}")]
        public async Task<IActionResult> GetKeyByReservation(int reservationId)
        {
            var key = await _digitalKeyService.GetKeyByReservationAsync(reservationId);
            if (key == null) return NotFound();
            return Ok(key);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveKeys()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var keys = await _digitalKeyService.GetActiveKeysAsync(userId);
            return Ok(keys);
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateKey([FromBody] ValidateKeyDto dto)
        {
            var isValid = await _digitalKeyService.ValidateKeyAsync(dto.KeyCode, dto.RoomId);
            return Ok(new { isValid });
        }

        [HttpPost("use")]
        public async Task<IActionResult> UseKey([FromBody] UseKeyDto dto)
        {
            var result = await _digitalKeyService.UseKeyAsync(dto.KeyCode);
            return Ok(new { success = result });
        }

        [HttpGet("qrcode/{reservationId}")]
        public async Task<IActionResult> GetQrCode(int reservationId)
        {
            var qrCode = await _digitalKeyService.GenerateQrCodeAsync(reservationId);
            if (qrCode == null) return NotFound();
            return Ok(new { qrCode });
        }
    }

    public class UseKeyDto
    {
        public string KeyCode { get; set; }
    }
}