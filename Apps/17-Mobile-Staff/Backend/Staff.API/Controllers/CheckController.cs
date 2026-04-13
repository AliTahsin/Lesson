using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Staff.API.DTOs;
using Staff.API.Services;

namespace Staff.API.Controllers
{
    [Authorize(Roles = "Admin,FrontDesk")]
    [ApiController]
    [Route("api/[controller]")]
    public class CheckController : ControllerBase
    {
        private readonly ICheckService _checkService;

        public CheckController(ICheckService checkService)
        {
            _checkService = checkService;
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInDto dto)
        {
            var staffId = int.Parse(User.FindFirst("staffId")?.Value ?? "0");
            var result = await _checkService.CheckInAsync(dto, staffId);
            return Ok(result);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] CheckOutDto dto)
        {
            var staffId = int.Parse(User.FindFirst("staffId")?.Value ?? "0");
            var result = await _checkService.CheckOutAsync(dto, staffId);
            return Ok(result);
        }

        [HttpGet("today-checkins")]
        public async Task<IActionResult> GetTodayCheckIns()
        {
            var hotelId = int.Parse(User.FindFirst("hotelId")?.Value ?? "0");
            var checks = await _checkService.GetTodayCheckInsAsync(hotelId);
            return Ok(checks);
        }

        [HttpGet("today-checkouts")]
        public async Task<IActionResult> GetTodayCheckOuts()
        {
            var hotelId = int.Parse(User.FindFirst("hotelId")?.Value ?? "0");
            var checks = await _checkService.GetTodayCheckOutsAsync(hotelId);
            return Ok(checks);
        }
    }
}