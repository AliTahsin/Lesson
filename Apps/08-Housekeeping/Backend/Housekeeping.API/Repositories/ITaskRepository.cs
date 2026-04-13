using Housekeeping.API.Models;

namespace Housekeeping.API.Repositories
{
    public interface ITaskRepository
    {
        Task<HousekeepingTask> GetByIdAsync(int id);
        Task<List<HousekeepingTask>> GetAllAsync();
        Task<List<HousekeepingTask>> GetByHotelAsync(int hotelId);
        Task<List<HousekeepingTask>> GetByRoomAsync(int roomId);
        Task<List<HousekeepingTask>> GetByStaffAsync(int staffId);
        Task<List<HousekeepingTask>> GetByStatusAsync(string status);
        Task<List<HousekeepingTask>> GetPendingTasksAsync();
        Task<HousekeepingTask> AddAsync(HousekeepingTask task);
        Task<HousekeepingTask> UpdateAsync(HousekeepingTask task);
        Task<bool> DeleteAsync(int id);
        Task<int> GetTaskCountByStatusAsync(string status);
        Task<Dictionary<string, int>> GetTaskStatisticsAsync(int hotelId);
    }
}