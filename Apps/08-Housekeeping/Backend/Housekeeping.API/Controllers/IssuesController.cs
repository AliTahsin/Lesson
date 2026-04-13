using Microsoft.AspNetCore.Mvc;
using Housekeeping.API.DTOs;
using Housekeeping.API.Services;

namespace Housekeeping.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssuesController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var issues = await _issueService.GetAllIssuesAsync();
            return Ok(issues);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var issue = await _issueService.GetIssueByIdAsync(id);
            if (issue == null) return NotFound();
            return Ok(issue);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var issues = await _issueService.GetIssuesByHotelAsync(hotelId);
            return Ok(issues);
        }

        [HttpGet("critical")]
        public async Task<IActionResult> GetCritical()
        {
            var issues = await _issueService.GetCriticalIssuesAsync();
            return Ok(issues);
        }

        [HttpPost]
        public async Task<IActionResult> Report([FromBody] CreateIssueDto dto)
        {
            var issue = await _issueService.ReportIssueAsync(dto);
            return Ok(issue);
        }

        [HttpPost("{id}/assign/{staffId}")]
        public async Task<IActionResult> Assign(int id, int staffId)
        {
            var issue = await _issueService.AssignIssueAsync(id, staffId);
            return Ok(issue);
        }

        [HttpPost("{id}/resolve")]
        public async Task<IActionResult> Resolve(int id, [FromBody] ResolveIssueDto dto)
        {
            var issue = await _issueService.ResolveIssueAsync(id, dto);
            return Ok(issue);
        }

        [HttpPost("{id}/close")]
        public async Task<IActionResult> Close(int id, [FromQuery] string notes)
        {
            var issue = await _issueService.CloseIssueAsync(id, notes);
            return Ok(issue);
        }
    }
}