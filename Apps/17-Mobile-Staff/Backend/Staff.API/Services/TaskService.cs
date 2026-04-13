using AutoMapper;
using Staff.API.Models;
using Staff.API.DTOs;
using Staff.API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Staff.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<TaskService> _logger;

        public TaskService(
            ITaskRepository taskRepository,
            IStaffRepository staffRepository,
            INotificationService notificationService,
            IMapper mapper,
            IHubContext<SignalRHub> hubContext,
            ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _staffRepository = staffRepository;
            _notificationService = notificationService;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task != null ? _mapper.Map<TaskDto>(task) : null;
        }

        public async Task<List<TaskDto>> GetTasksByStaffAsync(int staffId)
        {
            var tasks = await _taskRepository.GetByStaffAsync(staffId);
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<List<TaskDto>> GetPendingTasksAsync(int hotelId)
        {
            var tasks = await _taskRepository.GetPendingTasksAsync(hotelId);
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<TaskDto> AssignTaskAsync(int taskId, int staffId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new Exception("Task not found");

            var staff = await _staffRepository.GetByIdAsync(staffId);
            if (staff == null)
                throw new Exception("Staff not found");

            task.AssignedToStaffId = staffId;
            task.AssignedToStaffName = staff.FullName;
            task.Status = "Assigned";
            await _taskRepository.UpdateAsync(task);

            // Send notification to staff
            await _notificationService.SendTaskNotification(staffId, task.Id, task.TaskType);
            
            // Send real-time notification via SignalR
            await _hubContext.Clients.User(staffId.ToString()).SendAsync("NewTask", task);

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> StartTaskAsync(int taskId)
        {
            var task = await _taskRepository.UpdateStatusAsync(taskId, "InProgress");
            if (task == null)
                throw new Exception("Task not found");

            await _hubContext.Clients.Group($"hotel-{task.HotelId}").SendAsync("TaskStarted", task);

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CompleteTaskAsync(int taskId, CompleteTaskDto dto)
        {
            var task = await _taskRepository.UpdateStatusAsync(taskId, "Completed");
            if (task == null)
                throw new Exception("Task not found");

            task.Notes = dto.Notes;
            task.AfterImages = dto.AfterImages;
            if (task.StartedAt.HasValue)
            {
                task.ActualMinutes = (int)(DateTime.Now - task.StartedAt.Value).TotalMinutes;
            }
            await _taskRepository.UpdateAsync(task);

            await _hubContext.Clients.Group($"hotel-{task.HotelId}").SendAsync("TaskCompleted", task);

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto)
        {
            var task = new StaffTask
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
                EstimatedMinutes = dto.EstimatedMinutes ?? 30
            };

            await _taskRepository.AddAsync(task);
            
            await _hubContext.Clients.Group($"hotel-{dto.HotelId}").SendAsync("NewTaskCreated", task);

            return _mapper.Map<TaskDto>(task);
        }
    }
}