using Housekeeping.API.Models;
using Housekeeping.API.Data;

namespace Housekeeping.API.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly List<HousekeepingTask> _tasks;

        public TaskRepository()
        {
            _tasks = MockData.GetHousekeepingTasks();
        }

        public async Task<HousekeepingTask> GetByIdAsync(int id)
        {
            return await Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
        }

        public async Task<List<HousekeepingTask>> GetAllAsync()
        {
            return await Task.FromResult(_tasks.ToList());
        }

        public async Task<List<HousekeepingTask>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_tasks.Where(t => t.HotelId == hotelId).ToList());
        }

        public async Task<List<HousekeepingTask>> GetByRoomAsync(int roomId)
        {
            return await Task.FromResult(_tasks.Where(t => t.RoomId == roomId).ToList());
        }

        public async Task<List<HousekeepingTask>> GetByStaffAsync(int staffId)
        {
            return await Task.FromResult(_tasks.Where(t => t.AssignedToStaffId == staffId).ToList());
        }

        public async Task<List<HousekeepingTask>> GetByStatusAsync(string status)
        {
            return await Task.FromResult(_tasks.Where(t => t.Status == status).ToList());
        }

        public async Task<List<HousekeepingTask>> GetPendingTasksAsync()
        {
            return await Task.FromResult(_tasks.Where(t => t.Status == "Pending" || t.Status == "Assigned").ToList());
        }

        public async Task<HousekeepingTask> AddAsync(HousekeepingTask task)
        {
            task.Id = _tasks.Max(t => t.Id) + 1;
            _tasks.Add(task);
            return await Task.FromResult(task);
        }

        public async Task<HousekeepingTask> UpdateAsync(HousekeepingTask task)
        {
            var existing = await GetByIdAsync(task.Id);
            if (existing != null)
            {
                var index = _tasks.IndexOf(existing);
                _tasks[index] = task;
            }
            return await Task.FromResult(task);
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

        public async Task<int> GetTaskCountByStatusAsync(string status)
        {
            return await Task.FromResult(_tasks.Count(t => t.Status == status));
        }

        public async Task<Dictionary<string, int>> GetTaskStatisticsAsync(int hotelId)
        {
            var hotelTasks = _tasks.Where(t => t.HotelId == hotelId);
            return await Task.FromResult(hotelTasks
                .GroupBy(t => t.Status)
                .ToDictionary(g => g.Key, g => g.Count()));
        }
    }
}