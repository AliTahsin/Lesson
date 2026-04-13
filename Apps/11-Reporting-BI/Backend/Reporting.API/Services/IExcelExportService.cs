using Reporting.API.DTOs;

namespace Reporting.API.Services
{
    public interface IExcelExportService
    {
        Task<byte[]> ExportReportAsync(ReportRequestDto request);
    }
}