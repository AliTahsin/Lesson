using Staff.API.Models;

namespace Staff.API.Repositories
{
    public interface ITaskRepository
    {
        Task<StaffTask> GetByIdAsync(int id);
        Task<List<StaffTask>> GetAllAsync();
        Task<List<StaffTask>> GetByHotelAsync(int hotelId);
        Task<List<StaffTask>> GetByStaffAsync(int staffId);
        Task<List<StaffTask>> GetByStatusAsync(string status);
        Task<List<StaffTask>> GetPendingTasksAsync(int hotelId);
        Task<StaffTask> AddAsync(StaffTask task);
        Task<StaffTask> UpdateAsync(StaffTask task);
        Task<StaffTask> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);
        Task<int> GetTaskCountByStatusAsync(int hotelId, string status);
    }
}