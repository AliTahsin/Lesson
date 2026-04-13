using AutoMapper;
using Housekeeping.API.Models;
using Housekeeping.API.DTOs;
using Housekeeping.API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Housekeeping.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<TaskService> _logger;

        public TaskService(
            ITaskRepository taskRepository,
            IIssueRepository issueRepository,
            IMapper mapper,
            IHubContext<SignalRHub> hubContext,
            ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _issueRepository = issueRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task != null ? _mapper.Map<TaskDto>(task) : null;
        }

        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<List<TaskDto>> GetTasksByHotelAsync(int hotelId)
        {
            var tasks = await _taskRepository.GetByHotelAsync(hotelId);
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<List<TaskDto>> GetTasksByStaffAsync(int staffId)
        {
            var tasks = await _taskRepository.GetByStaffAsync(staffId);
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<List<TaskDto>> GetPendingTasksAsync()
        {
            var tasks = await _taskRepository.GetPendingTasksAsync();
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto)
        {
            var task = new HousekeepingTask
            {
                TaskNumber = $"TASK-{DateTime.Now.Ticks}",
                HotelId = dto.HotelId,
                RoomId = dto.RoomId,
                RoomNumber = dto.RoomNumber,
                TaskType = dto.TaskType,
                Priority = dto.Priority,
                Description = dto.Description,
                Status = "Pending",
                CreatedAt = DateTime.Now,
                ScheduledDate = dto.ScheduledDate,
                EstimatedMinutes = dto.EstimatedMinutes ?? 30,
                Notes = dto.Notes
            };

            await _taskRepository.AddAsync(task);
            
            // Send real-time notification via SignalR
            await _hubContext.Clients.Group($"hotel-{dto.HotelId}").SendAsync("NewTask", task);
            
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> AssignTaskAsync(int taskId, int staffId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new Exception("Task not found");

            task.AssignedToStaffId = staffId;
            task.AssignedToStaffName = $"Staff {staffId}";
            task.Status = "Assigned";
            await _taskRepository.UpdateAsync(task);
            
            // Send notification to assigned staff
            await _hubContext.Clients.User(staffId.ToString()).SendAsync("TaskAssigned", task);
            
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> StartTaskAsync(int taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new Exception("Task not found");

            task.Status = "InProgress";
            task.StartedAt = DateTime.Now;
            await _taskRepository.UpdateAsync(task);
            
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CompleteTaskAsync(int taskId, CompleteTaskDto dto)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new Exception("Task not found");

            task.Status = "Completed";
            task.CompletedAt = DateTime.Now;
            task.Notes = dto.Notes;
            task.AfterImages = dto.AfterImages;
            
            if (task.StartedAt.HasValue)
            {
                task.ActualMinutes = (int)(DateTime.Now - task.StartedAt.Value).TotalMinutes;
            }
            
            await _taskRepository.UpdateAsync(task);
            
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CancelTaskAsync(int taskId, string reason)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new Exception("Task not found");

            task.Status = "Cancelled";
            task.Notes = $"Cancelled: {reason}";
            await _taskRepository.UpdateAsync(task);
            
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync(int hotelId)
        {
            var tasks = await _taskRepository.GetByHotelAsync(hotelId);
            var issues = await _issueRepository.GetByHotelAsync(hotelId);
            
            var today = DateTime.Today;
            var todayTasks = tasks.Where(t => t.CreatedAt.Date == today).ToList();
            var pendingTasks = tasks.Where(t => t.Status == "Pending" || t.Status == "Assigned").ToList();
            var inProgressTasks = tasks.Where(t => t.Status == "InProgress").ToList();
            var completedToday = tasks.Where(t => t.CompletedAt.HasValue && t.CompletedAt.Value.Date == today).ToList();
            
            var criticalIssues = issues.Where(i => i.Priority == "Critical" && i.Status != "Resolved").ToList();
            var openIssues = issues.Where(i => i.Status != "Resolved" && i.Status != "Closed").ToList();
            
            return new DashboardStatsDto
            {
                TotalTasksToday = todayTasks.Count,
                PendingTasks = pendingTasks.Count,
                InProgressTasks = inProgressTasks.Count,
                CompletedToday = completedToday.Count,
                CompletionRate = todayTasks.Any() ? (decimal)completedToday.Count / todayTasks.Count * 100 : 0,
                AverageTaskTime = tasks.Where(t => t.ActualMinutes > 0).Average(t => t.ActualMinutes),
                CriticalIssues = criticalIssues.Count,
                OpenIssues = openIssues.Count,
                AverageResolutionTime = await _issueRepository.GetAverageResolutionTimeAsync(),
                TasksByType = tasks.GroupBy(t => t.TaskType).ToDictionary(g => g.Key, g => g.Count()),
                IssuesByCategory = issues.GroupBy(i => i.Category).ToDictionary(g => g.Key, g => g.Count())
            };
        }
    }
}