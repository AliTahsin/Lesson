using Staff.API.DTOs;

namespace Staff.API.Services
{
    public interface ITaskService
    {
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task<List<TaskDto>> GetTasksByStaffAsync(int staffId);
        Task<List<TaskDto>> GetPendingTasksAsync(int hotelId);
        Task<TaskDto> AssignTaskAsync(int taskId, int staffId);
        Task<TaskDto> StartTaskAsync(int taskId);
        Task<TaskDto> CompleteTaskAsync(int taskId, CompleteTaskDto dto);
        Task<TaskDto> CreateTaskAsync(CreateTaskDto dto);
    }
}