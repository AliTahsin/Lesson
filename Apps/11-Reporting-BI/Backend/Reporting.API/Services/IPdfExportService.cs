using Reporting.API.DTOs;

namespace Reporting.API.Services
{
    public interface IPdfExportService
    {
        Task<byte[]> ExportReportAsync(ReportRequestDto request);
    }
}