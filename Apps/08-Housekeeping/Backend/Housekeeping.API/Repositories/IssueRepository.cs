using Housekeeping.API.Models;
using Housekeeping.API.Data;

namespace Housekeeping.API.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly List<MaintenanceIssue> _issues;

        public IssueRepository()
        {
            _issues = MockData.GetMaintenanceIssues();
        }

        public async Task<MaintenanceIssue> GetByIdAsync(int id)
        {
            return await Task.FromResult(_issues.FirstOrDefault(i => i.Id == id));
        }

        public async Task<List<MaintenanceIssue>> GetAllAsync()
        {
            return await Task.FromResult(_issues.ToList());
        }

        public async Task<List<MaintenanceIssue>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_issues.Where(i => i.HotelId == hotelId).ToList());
        }

        public async Task<List<MaintenanceIssue>> GetByRoomAsync(int roomId)
        {
            return await Task.FromResult(_issues.Where(i => i.RoomId == roomId).ToList());
        }

        public async Task<List<MaintenanceIssue>> GetByStatusAsync(string status)
        {
            return await Task.FromResult(_issues.Where(i => i.Status == status).ToList());
        }

        public async Task<List<MaintenanceIssue>> GetCriticalIssuesAsync()
        {
            return await Task.FromResult(_issues.Where(i => i.Priority == "Critical" && i.Status != "Resolved" && i.Status != "Closed").ToList());
        }

        public async Task<MaintenanceIssue> AddAsync(MaintenanceIssue issue)
        {
            issue.Id = _issues.Max(i => i.Id) + 1;
            _issues.Add(issue);
            return await Task.FromResult(issue);
        }

        public async Task<MaintenanceIssue> UpdateAsync(MaintenanceIssue issue)
        {
            var existing = await GetByIdAsync(issue.Id);
            if (existing != null)
            {
                var index = _issues.IndexOf(existing);
                _issues[index] = issue;
            }
            return await Task.FromResult(issue);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var issue = await GetByIdAsync(id);
            if (issue != null)
            {
                _issues.Remove(issue);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<int> GetAverageResolutionTimeAsync()
        {
            var resolvedIssues = _issues.Where(i => i.ResolvedAt.HasValue);
            if (!resolvedIssues.Any())
                return 0;
            
            var avgHours = resolvedIssues.Average(i => (i.ResolvedAt.Value - i.ReportedAt).TotalHours);
            return await Task.FromResult((int)avgHours);
        }
    }
}