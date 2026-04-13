using Staff.API.Models;

namespace Staff.API.Repositories
{
    public interface IIssueRepository
    {
        Task<MaintenanceIssue> GetByIdAsync(int id);
        Task<List<MaintenanceIssue>> GetAllAsync();
        Task<List<MaintenanceIssue>> GetByHotelAsync(int hotelId);
        Task<List<MaintenanceIssue>> GetByStaffAsync(int staffId);
        Task<List<MaintenanceIssue>> GetCriticalIssuesAsync(int hotelId);
        Task<MaintenanceIssue> AddAsync(MaintenanceIssue issue);
        Task<MaintenanceIssue> UpdateAsync(MaintenanceIssue issue);
        Task<MaintenanceIssue> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);
    }
}