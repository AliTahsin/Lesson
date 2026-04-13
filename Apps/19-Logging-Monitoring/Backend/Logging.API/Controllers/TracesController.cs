using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logging.API.Services;

namespace Logging.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TracesController : ControllerBase
    {
        private readonly ITracingService _tracingService;

        public TracesController(ITracingService tracingService)
        {
            _tracingService = tracingService;
        }

        [HttpGet("{traceId}")]
        public async Task<IActionResult> GetById(string traceId)
        {
            var trace = await _tracingService.GetTraceByIdAsync(traceId);
            if (trace == null) return NotFound();
            return Ok(trace);
        }

        [HttpGet("service/{service}")]
        public async Task<IActionResult> GetByService(string service, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var traces = await _tracingService.GetTracesByServiceAsync(service, startDate, endDate);
            return Ok(traces);
        }

        [HttpGet("slow")]
        public async Task<IActionResult> GetSlowTraces([FromQuery] long minDurationMs = 1000, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var start = startDate ?? DateTime.Now.AddDays(-1);
            var end = endDate ?? DateTime.Now;
            var traces = await _tracingService.GetSlowTracesAsync(minDurationMs, start, end);
            return Ok(traces);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var stats = await _tracingService.GetTraceStatisticsAsync(startDate, endDate);
            return Ok(stats);
        }
    }
}