using Microsoft.AspNetCore.Mvc;
using Reporting.API.DTOs;
using Reporting.API.Services;

namespace Reporting.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueReport(
            [FromQuery] int hotelId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var report = await _reportService.GetRevenueReportAsync(hotelId, startDate, endDate);
            return Ok(report);
        }

        [HttpGet("occupancy")]
        public async Task<IActionResult> GetOccupancyReport(
            [FromQuery] int hotelId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var report = await _reportService.GetOccupancyReportAsync(hotelId, startDate, endDate);
            return Ok(report);
        }

        [HttpGet("reservation")]
        public async Task<IActionResult> GetReservationReport(
            [FromQuery] int hotelId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var report = await _reportService.GetReservationReportAsync(hotelId, startDate, endDate);
            return Ok(report);
        }

        [HttpGet("customer")]
        public async Task<IActionResult> GetCustomerReport(
            [FromQuery] int hotelId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var report = await _reportService.GetCustomerReportAsync(hotelId, startDate, endDate);
            return Ok(report);
        }

        [HttpGet("channel")]
        public async Task<IActionResult> GetChannelReport(
            [FromQuery] int hotelId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var report = await _reportService.GetChannelReportAsync(hotelId, startDate, endDate);
            return Ok(report);
        }

        [HttpPost("export/excel")]
        public async Task<IActionResult> ExportToExcel([FromBody] ReportRequestDto request)
        {
            var excelData = await _reportService.ExportToExcelAsync(request);
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"{request.ReportType}_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        [HttpPost("export/pdf")]
        public async Task<IActionResult> ExportToPdf([FromBody] ReportRequestDto request)
        {
            var pdfData = await _reportService.ExportToPdfAsync(request);
            return File(pdfData, "application/pdf", 
                $"{request.ReportType}_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateAndSave([FromBody] ReportRequestDto request)
        {
            var report = await _reportService.GenerateAndSaveReportAsync(request, 1, "Admin");
            return Ok(report);
        }
    }
}