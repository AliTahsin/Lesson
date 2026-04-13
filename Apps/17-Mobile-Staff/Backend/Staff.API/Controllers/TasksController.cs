using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Staff.API.DTOs;
using Staff.API.Services;

namespace Staff.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            var staffId = int.Parse(User.FindFirst("staffId")?.Value ?? "0");
            var tasks = await _taskService.GetTasksByStaffAsync(staffId);
            return Ok(tasks);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingTasks()
        {
            var hotelId = int.Parse(User.FindFirst("hotelId")?.Value ?? "0");
            var tasks = await _taskService.GetPendingTasksAsync(hotelId);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartTask(int id)
        {
            var task = await _taskService.StartTaskAsync(id);
            return Ok(task);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteTask(int id, [FromBody] CompleteTaskDto dto)
        {
            var task = await _taskService.CompleteTaskAsync(id, dto);
            return Ok(task);
        }

        [Authorize(Roles = "Admin,FrontDesk")]
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            var task = await _taskService.CreateTaskAsync(dto);
            return Ok(task);
        }

        [Authorize(Roles = "Admin,FrontDesk")]
        [HttpPost("{id}/assign/{staffId}")]
        public async Task<IActionResult> AssignTask(int id, int staffId)
        {
            var task = await _taskService.AssignTaskAsync(id, staffId);
            return Ok(task);
        }
    }
}