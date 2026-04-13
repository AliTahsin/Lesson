using Reporting.API.DTOs;

namespace Reporting.API.Services
{
    public interface IReportService
    {
        Task<RevenueReportDto> GetRevenueReportAsync(int hotelId, DateTime startDate, DateTime endDate);
        Task<OccupancyReportDto> GetOccupancyReportAsync(int hotelId, DateTime startDate, DateTime endDate);
        Task<ReservationReportDto> GetReservationReportAsync(int hotelId, DateTime startDate, DateTime endDate);
        Task<CustomerReportDto> GetCustomerReportAsync(int hotelId, DateTime startDate, DateTime endDate);
        Task<ChannelReportDto> GetChannelReportAsync(int hotelId, DateTime startDate, DateTime endDate);
        Task<byte[]> ExportToExcelAsync(ReportRequestDto request);
        Task<byte[]> ExportToPdfAsync(ReportRequestDto request);
        Task<ReportDto> GenerateAndSaveReportAsync(ReportRequestDto request, int userId, string userName);
    }
}