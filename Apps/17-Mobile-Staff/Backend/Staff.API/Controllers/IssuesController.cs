using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Staff.API.DTOs;
using Staff.API.Services;

namespace Staff.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssuesController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet("my-issues")]
        public async Task<IActionResult> GetMyIssues()
        {
            var staffId = int.Parse(User.FindFirst("staffId")?.Value ?? "0");
            var issues = await _issueService.GetIssuesByStaffAsync(staffId);
            return Ok(issues);
        }

        [HttpGet("critical")]
        public async Task<IActionResult> GetCriticalIssues()
        {
            var hotelId = int.Parse(User.FindFirst("hotelId")?.Value ?? "0");
            var issues = await _issueService.GetCriticalIssuesAsync(hotelId);
            return Ok(issues);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var issue = await _issueService.GetIssueByIdAsync(id);
            if (issue == null) return NotFound();
            return Ok(issue);
        }

        [HttpPost]
        public async Task<IActionResult> ReportIssue([FromBody] CreateIssueDto dto)
        {
            var staffId = int.Parse(User.FindFirst("staffId")?.Value ?? "0");
            dto.ReportedByStaffId = staffId;
            dto.ReportedByName = User.FindFirst("name")?.Value ?? "Staff";
            var issue = await _issueService.ReportIssueAsync(dto);
            return Ok(issue);
        }

        [HttpPost("{id}/assign/{staffId}")]
        public async Task<IActionResult> AssignIssue(int id, int staffId)
        {
            var issue = await _issueService.AssignIssueAsync(id, staffId);
            return Ok(issue);
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartIssue(int id)
        {
            var issue = await _issueService.StartIssueAsync(id);
            return Ok(issue);
        }

        [HttpPost("{id}/resolve")]
        public async Task<IActionResult> ResolveIssue(int id, [FromBody] ResolveIssueDto dto)
        {
            var issue = await _issueService.ResolveIssueAsync(id, dto);
            return Ok(issue);
        }
    }
}