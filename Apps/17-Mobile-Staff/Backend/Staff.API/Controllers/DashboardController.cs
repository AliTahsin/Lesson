using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Staff.API.DTOs;
using Staff.API.Repositories;

namespace Staff.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly ICheckRepository _checkRepository;

        public DashboardController(
            ITaskRepository taskRepository,
            IIssueRepository issueRepository,
            ICheckRepository checkRepository)
        {
            _taskRepository = taskRepository;
            _issueRepository = issueRepository;
            _checkRepository = checkRepository;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var hotelId = int.Parse(User.FindFirst("hotelId")?.Value ?? "0");
            var role = User.FindFirst("role")?.Value;

            var today = DateTime.Today;
            var todayTasks = await _taskRepository.GetByHotelAsync(hotelId);
            var todayFiltered = todayTasks.Where(t => t.CreatedAt.Date == today).ToList();

            var stats = new DashboardStatsDto
            {
                TotalTasksToday = todayFiltered.Count,
                PendingTasks = todayTasks.Count(t => t.Status == "Pending" || t.Status == "Assigned"),
                InProgressTasks = todayTasks.Count(t => t.Status == "InProgress"),
                CompletedToday = todayFiltered.Count(t => t.Status == "Completed"),
                CompletionRate = todayFiltered.Any() ? (decimal)todayFiltered.Count(t => t.Status == "Completed") / todayFiltered.Count * 100 : 0,
                CriticalIssues = (await _issueRepository.GetCriticalIssuesAsync(hotelId)).Count,
                OpenIssues = (await _issueRepository.GetByHotelAsync(hotelId)).Count(i => i.Status != "Resolved" && i.Status != "Closed"),
                TodayCheckIns = (await _checkRepository.GetByDateAsync(today)).Count(c => c.Type == "CheckIn"),
                TodayCheckOuts = (await _checkRepository.GetByDateAsync(today)).Count(c => c.Type == "CheckOut"),
                TasksByType = todayTasks.GroupBy(t => t.TaskType).ToDictionary(g => g.Key, g => g.Count()),
                IssuesByCategory = (await _issueRepository.GetByHotelAsync(hotelId)).GroupBy(i => i.Category).ToDictionary(g => g.Key, g => g.Count())
            };

            return Ok(stats);
        }
    }
}