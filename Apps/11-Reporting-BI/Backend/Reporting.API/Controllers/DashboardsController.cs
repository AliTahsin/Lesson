using Microsoft.AspNetCore.Mvc;
using Reporting.API.DTOs;
using Reporting.API.Services;
using Reporting.API.Repositories;

namespace Reporting.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IReportService _reportService;
        private readonly ILogger<DashboardsController> _logger;

        public DashboardsController(
            IDashboardRepository dashboardRepository,
            IReportService reportService,
            ILogger<DashboardsController> logger)
        {
            _dashboardRepository = dashboardRepository;
            _reportService = reportService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dashboards = await _dashboardRepository.GetByHotelAsync(1); // Default hotel
            return Ok(dashboards);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dashboard = await _dashboardRepository.GetByIdAsync(id);
            if (dashboard == null) return NotFound();
            return Ok(dashboard);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var dashboards = await _dashboardRepository.GetByHotelAsync(hotelId);
            return Ok(dashboards);
        }

        [HttpGet("hotel/{hotelId}/type/{dashboardType}")]
        public async Task<IActionResult> GetDefaultDashboard(int hotelId, string dashboardType)
        {
            var dashboard = await _dashboardRepository.GetDefaultDashboardAsync(hotelId, dashboardType);
            if (dashboard == null) return NotFound();
            return Ok(dashboard);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Dashboard dashboard)
        {
            var created = await _dashboardRepository.AddAsync(dashboard);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Dashboard dashboard)
        {
            dashboard.Id = id;
            var updated = await _dashboardRepository.UpdateAsync(dashboard);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dashboardRepository.DeleteAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Dashboard deleted successfully" });
        }

        [HttpGet("kpis/hotel/{hotelId}")]
        public async Task<IActionResult> GetKPIs(int hotelId)
        {
            var kpis = Reporting.API.Data.MockData.GetKPIs().Where(k => k.HotelId == hotelId).ToList();
            return Ok(kpis);
        }

        [HttpGet("kpis/{id}")]
        public async Task<IActionResult> GetKPIById(int id)
        {
            var kpi = Reporting.API.Data.MockData.GetKPIs().FirstOrDefault(k => k.Id == id);
            if (kpi == null) return NotFound();
            return Ok(kpi);
        }

        [HttpGet("widgets/{dashboardId}/data")]
        public async Task<IActionResult> GetWidgetData(int dashboardId, [FromQuery] string widgetId)
        {
            // Mock widget data based on widget type
            var random = new Random();
            
            var widgetData = new
            {
                DashboardId = dashboardId,
                WidgetId = widgetId,
                Data = new
                {
                    Labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                    Values = Enumerable.Range(1, 7).Select(x => random.Next(1000, 10000)).ToArray(),
                    Total = random.Next(50000, 200000),
                    Change = random.Next(-15, 25),
                    Trend = random.Next(0, 10) > 7 ? "up" : "down"
                },
                Timestamp = DateTime.Now
            };
            
            return Ok(widgetData);
        }
    }
}