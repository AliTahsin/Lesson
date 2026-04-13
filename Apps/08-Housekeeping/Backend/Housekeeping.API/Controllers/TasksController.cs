using Microsoft.AspNetCore.Mvc;
using Housekeeping.API.DTOs;
using Housekeeping.API.Services;

namespace Housekeeping.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var tasks = await _taskService.GetTasksByHotelAsync(hotelId);
            return Ok(tasks);
        }

        [HttpGet("staff/{staffId}")]
        public async Task<IActionResult> GetByStaff(int staffId)
        {
            var tasks = await _taskService.GetTasksByStaffAsync(staffId);
            return Ok(tasks);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var tasks = await _taskService.GetPendingTasksAsync();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var task = await _taskService.CreateTaskAsync(dto);
            return Ok(task);
        }

        [HttpPost("{id}/assign/{staffId}")]
        public async Task<IActionResult> Assign(int id, int staffId)
        {
            var task = await _taskService.AssignTaskAsync(id, staffId);
            return Ok(task);
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> Start(int id)
        {
            var task = await _taskService.StartTaskAsync(id);
            return Ok(task);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id, [FromBody] CompleteTaskDto dto)
        {
            var task = await _taskService.CompleteTaskAsync(id, dto);
            return Ok(task);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id, [FromQuery] string reason)
        {
            var task = await _taskService.CancelTaskAsync(id, reason);
            return Ok(task);
        }
    }
}