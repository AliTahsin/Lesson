using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logging.API.DTOs;
using Logging.API.Services;

namespace Logging.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILoggingService _loggingService;

        public LogsController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] LogSearchDto searchDto)
        {
            var logs = await _loggingService.SearchLogsAsync(searchDto);
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var log = await _loggingService.GetLogByIdAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var stats = await _loggingService.GetLogStatisticsAsync(startDate, endDate);
            return Ok(stats);
        }

        [HttpGet("errors")]
        public async Task<IActionResult> GetErrors([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var logs = await _loggingService.GetErrorLogsAsync(startDate, endDate);
            return Ok(logs);
        }

        [HttpGet("service/{service}")]
        public async Task<IActionResult> GetByService(string service, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var logs = await _loggingService.GetLogsByServiceAsync(service, startDate, endDate);
            return Ok(logs);
        }

        [HttpGet("levels")]
        public async Task<IActionResult> GetCountByLevel([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var counts = await _loggingService.GetLogCountByLevelAsync(startDate, endDate);
            return Ok(counts);
        }
    }
}