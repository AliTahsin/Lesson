using Housekeeping.API.Models;

namespace Housekeeping.API.Repositories
{
    public interface IIssueRepository
    {
        Task<MaintenanceIssue> GetByIdAsync(int id);
        Task<List<MaintenanceIssue>> GetAllAsync();
        Task<List<MaintenanceIssue>> GetByHotelAsync(int hotelId);
        Task<List<MaintenanceIssue>> GetByRoomAsync(int roomId);
        Task<List<MaintenanceIssue>> GetByStatusAsync(string status);
        Task<List<MaintenanceIssue>> GetCriticalIssuesAsync();
        Task<MaintenanceIssue> AddAsync(MaintenanceIssue issue);
        Task<MaintenanceIssue> UpdateAsync(MaintenanceIssue issue);
        Task<bool> DeleteAsync(int id);
        Task<int> GetAverageResolutionTimeAsync();
    }
}