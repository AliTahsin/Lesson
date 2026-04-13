using Reporting.API.Models;

namespace Reporting.API.Repositories
{
    public interface IDashboardRepository
    {
        Task<Dashboard> GetByIdAsync(int id);
        Task<List<Dashboard>> GetByHotelAsync(int hotelId);
        Task<Dashboard> GetDefaultDashboardAsync(int hotelId, string dashboardType);
        Task<Dashboard> AddAsync(Dashboard dashboard);
        Task<Dashboard> UpdateAsync(Dashboard dashboard);
        Task<bool> DeleteAsync(int id);
    }
}