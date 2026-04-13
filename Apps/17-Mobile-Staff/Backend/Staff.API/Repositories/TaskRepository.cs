using Staff.API.Models;
using Staff.API.Data;

namespace Staff.API.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly List<StaffTask> _tasks;

        public TaskRepository()
        {
            _tasks = MockData.GetStaffTasks();
        }

        public async Task<StaffTask> GetByIdAsync(int id)
        {
            return await Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
        }

        public async Task<List<StaffTask>> GetAllAsync()
        {
            return await Task.FromResult(_tasks.ToList());
        }

        public async Task<List<StaffTask>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_tasks.Where(t => t.HotelId == hotelId).ToList());
        }

        public async Task<List<StaffTask>> GetByStaffAsync(int staffId)
        {
            return await Task.FromResult(_tasks.Where(t => t.AssignedToStaffId == staffId).ToList());
        }

        public async Task<List<StaffTask>> GetByStatusAsync(string status)
        {
            return await Task.FromResult(_tasks.Where(t => t.Status == status).ToList());
        }

        public async Task<List<StaffTask>> GetPendingTasksAsync(int hotelId)
        {
            return await Task.FromResult(_tasks
                .Where(t => t.HotelId == hotelId && (t.Status == "Pending" || t.Status == "Assigned"))
                .ToList());
        }

        public async Task<StaffTask> AddAsync(StaffTask task)
        {
            task.Id = _tasks.Max(t => t.Id) + 1;
            task.CreatedAt = DateTime.Now;
            _tasks.Add(task);
            return await Task.FromResult(task);
        }

        public async Task<StaffTask> UpdateAsync(StaffTask task)
        {
            var existing = await GetByIdAsync(task.Id);
            if (existing != null)
            {
                var index = _tasks.IndexOf(existing);
                _tasks[index] = task;
            }
            return await Task.FromResult(task);
        }

        public async Task<StaffTask> UpdateStatusAsync(int id, string status)
        {
            var task = await GetByIdAsync(id);
            if (task != null)
            {
                task.Status = status;
                if (status == "InProgress") task.StartedAt = DateTime.Now;
                if (status == "Completed") task.CompletedAt = DateTime.Now;
                return await Task.FromResult(task);
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await GetByIdAsync(id);
            if (task != null)
            {
                _tasks.Remove(task);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<int> GetTaskCountByStatusAsync(int hotelId, string status)
        {
            return await Task.FromResult(_tasks.Count(t => t.HotelId == hotelId && t.Status == status));
        }
    }
}