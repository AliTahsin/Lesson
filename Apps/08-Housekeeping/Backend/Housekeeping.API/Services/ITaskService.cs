using Housekeeping.API.DTOs;

namespace Housekeeping.API.Services
{
    public interface ITaskService
    {
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<List<TaskDto>> GetTasksByHotelAsync(int hotelId);
        Task<List<TaskDto>> GetTasksByStaffAsync(int staffId);
        Task<List<TaskDto>> GetPendingTasksAsync();
        Task<TaskDto> CreateTaskAsync(CreateTaskDto dto);
        Task<TaskDto> AssignTaskAsync(int taskId, int staffId);
        Task<TaskDto> StartTaskAsync(int taskId);
        Task<TaskDto> CompleteTaskAsync(int taskId, CompleteTaskDto dto);
        Task<TaskDto> CancelTaskAsync(int taskId, string reason);
        Task<DashboardStatsDto> GetDashboardStatsAsync(int hotelId);
    }
}