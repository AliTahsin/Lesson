using Microsoft.AspNetCore.Mvc;
using Housekeeping.API.Services;

namespace Housekeeping.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public DashboardController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("stats/{hotelId}")]
        public async Task<IActionResult> GetStats(int hotelId)
        {
            var stats = await _taskService.GetDashboardStatsAsync(hotelId);
            return Ok(stats);
        }
    }
}