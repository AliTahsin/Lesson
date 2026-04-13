using Reporting.API.Models;

namespace Reporting.API.Repositories
{
    public interface IReportRepository
    {
        Task<Report> GetByIdAsync(int id);
        Task<List<Report>> GetAllAsync();
        Task<List<Report>> GetByHotelAsync(int hotelId);
        Task<List<Report>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Report> AddAsync(Report report);
        Task<Report> UpdateAsync(Report report);
        Task<bool> DeleteAsync(int id);
    }
}