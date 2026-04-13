using Microsoft.AspNetCore.Mvc;
using Reporting.API.Models;
using Reporting.API.DTOs;
using Reporting.API.Services;

namespace Reporting.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduledReportsController : ControllerBase
    {
        private readonly ILogger<ScheduledReportsController> _logger;
        private static List<ScheduledReport> _scheduledReports = new();

        public ScheduledReportsController(ILogger<ScheduledReportsController> logger)
        {
            _logger = logger;
            if (!_scheduledReports.Any())
            {
                _scheduledReports = GetMockScheduledReports();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Task.FromResult(_scheduledReports));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var report = _scheduledReports.FirstOrDefault(r => r.Id == id);
            if (report == null) return NotFound();
            return Ok(await Task.FromResult(report));
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var reports = _scheduledReports.Where(r => r.ReportId > 0).ToList(); // Simplified
            return Ok(await Task.FromResult(reports));
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var reports = _scheduledReports.Where(r => r.IsActive).ToList();
            return Ok(await Task.FromResult(reports));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ScheduledReport report)
        {
            report.Id = _scheduledReports.Max(r => r.Id) + 1;
            report.CreatedAt = DateTime.Now;
            report.NextRunAt = CalculateNextRun(report);
            _scheduledReports.Add(report);
            return Ok(await Task.FromResult(report));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ScheduledReport report)
        {
            var existing = _scheduledReports.FirstOrDefault(r => r.Id == id);
            if (existing == null) return NotFound();
            
            existing.Name = report.Name;
            existing.Frequency = report.Frequency;
            existing.DayOfWeek = report.DayOfWeek;
            existing.DayOfMonth = report.DayOfMonth;
            existing.TimeOfDay = report.TimeOfDay;
            existing.RecipientEmails = report.RecipientEmails;
            existing.Format = report.Format;
            existing.IsActive = report.IsActive;
            existing.UpdatedAt = DateTime.Now;
            existing.NextRunAt = CalculateNextRun(existing);
            
            return Ok(await Task.FromResult(existing));
        }

        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var report = _scheduledReports.FirstOrDefault(r => r.Id == id);
            if (report == null) return NotFound();
            
            report.IsActive = true;
            report.NextRunAt = CalculateNextRun(report);
            report.UpdatedAt = DateTime.Now;
            
            return Ok(await Task.FromResult(report));
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var report = _scheduledReports.FirstOrDefault(r => r.Id == id);
            if (report == null) return NotFound();
            
            report.IsActive = false;
            report.UpdatedAt = DateTime.Now;
            
            return Ok(await Task.FromResult(report));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var report = _scheduledReports.FirstOrDefault(r => r.Id == id);
            if (report == null) return NotFound();
            
            _scheduledReports.Remove(report);
            return Ok(new { message = "Scheduled report deleted successfully" });
        }

        [HttpPost("{id}/run")]
        public async Task<IActionResult> RunNow(int id)
        {
            var report = _scheduledReports.FirstOrDefault(r => r.Id == id);
            if (report == null) return NotFound();
            
            // Simulate report generation
            report.LastRunAt = DateTime.Now;
            report.LastRunStatus = "Success";
            report.NextRunAt = CalculateNextRun(report);
            
            _logger.LogInformation($"Scheduled report {report.Name} executed at {DateTime.Now}");
            
            return Ok(new { message = "Report generation started", report });
        }

        private DateTime CalculateNextRun(ScheduledReport report)
        {
            var now = DateTime.Now;
            var timeParts = report.TimeOfDay?.Split(':') ?? new[] { "09", "00" };
            var hour = int.Parse(timeParts[0]);
            var minute = int.Parse(timeParts[1]);

            return report.Frequency switch
            {
                "Daily" => new DateTime(now.Year, now.Month, now.Day, hour, minute, 0).AddDays(1),
                "Weekly" => GetNextWeekday(now, report.DayOfWeek, hour, minute),
                "Monthly" => GetNextMonthDay(now, report.DayOfMonth, hour, minute),
                _ => now.AddDays(1)
            };
        }

        private DateTime GetNextWeekday(DateTime from, string dayOfWeek, int hour, int minute)
        {
            var targetDay = dayOfWeek switch
            {
                "Monday" => DayOfWeek.Monday,
                "Tuesday" => DayOfWeek.Tuesday,
                "Wednesday" => DayOfWeek.Wednesday,
                "Thursday" => DayOfWeek.Thursday,
                "Friday" => DayOfWeek.Friday,
                "Saturday" => DayOfWeek.Saturday,
                "Sunday" => DayOfWeek.Sunday,
                _ => DayOfWeek.Monday
            };

            var daysToAdd = ((int)targetDay - (int)from.DayOfWeek + 7) % 7;
            if (daysToAdd == 0) daysToAdd = 7;
            
            return new DateTime(from.Year, from.Month, from.Day, hour, minute, 0).AddDays(daysToAdd);
        }

        private DateTime GetNextMonthDay(DateTime from, int dayOfMonth, int hour, int minute)
        {
            var nextMonth = from.AddMonths(1);
            var targetDay = Math.Min(dayOfMonth, DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month));
            return new DateTime(nextMonth.Year, nextMonth.Month, targetDay, hour, minute, 0);
        }

        private List<ScheduledReport> GetMockScheduledReports()
        {
            return new List<ScheduledReport>
            {
                new ScheduledReport
                {
                    Id = 1,
                    Name = "Daily Revenue Report",
                    ReportId = 1,
                    Frequency = "Daily",
                    TimeOfDay = "09:00",
                    RecipientEmails = "admin@hotel.com, finance@hotel.com",
                    Format = "PDF",
                    IsActive = true,
                    LastRunAt = DateTime.Now.AddDays(-1),
                    NextRunAt = DateTime.Now.AddDays(1),
                    LastRunStatus = "Success",
                    CreatedAt = DateTime.Now.AddMonths(-2)
                },
                new ScheduledReport
                {
                    Id = 2,
                    Name = "Weekly Occupancy Report",
                    ReportId = 2,
                    Frequency = "Weekly",
                    DayOfWeek = "Monday",
                    TimeOfDay = "08:00",
                    RecipientEmails = "operations@hotel.com",
                    Format = "Excel",
                    IsActive = true,
                    LastRunAt = DateTime.Now.AddDays(-7),
                    NextRunAt = GetNextWeekday(DateTime.Now, "Monday", 8, 0),
                    LastRunStatus = "Success",
                    CreatedAt = DateTime.Now.AddMonths(-2)
                },
                new ScheduledReport
                {
                    Id = 3,
                    Name = "Monthly Customer Report",
                    ReportId = 4,
                    Frequency = "Monthly",
                    DayOfMonth = 1,
                    TimeOfDay = "10:00",
                    RecipientEmails = "marketing@hotel.com",
                    Format = "PDF",
                    IsActive = false,
                    LastRunAt = DateTime.Now.AddMonths(-1),
                    NextRunAt = GetNextMonthDay(DateTime.Now, 1, 10, 0),
                    LastRunStatus = "Success",
                    CreatedAt = DateTime.Now.AddMonths(-3)
                }
            };
        }
    }
}