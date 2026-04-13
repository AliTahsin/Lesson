using Staff.API.Models;
using Staff.API.Data;

namespace Staff.API.Repositories
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

        public async Task<List<MaintenanceIssue>> GetByStaffAsync(int staffId)
        {
            return await Task.FromResult(_issues.Where(i => i.AssignedToStaffId == staffId).ToList());
        }

        public async Task<List<MaintenanceIssue>> GetCriticalIssuesAsync(int hotelId)
        {
            return await Task.FromResult(_issues
                .Where(i => i.HotelId == hotelId && i.Priority == "Critical" && i.Status != "Resolved")
                .ToList());
        }

        public async Task<MaintenanceIssue> AddAsync(MaintenanceIssue issue)
        {
            issue.Id = _issues.Max(i => i.Id) + 1;
            issue.ReportedAt = DateTime.Now;
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

        public async Task<MaintenanceIssue> UpdateStatusAsync(int id, string status)
        {
            var issue = await GetByIdAsync(id);
            if (issue != null)
            {
                issue.Status = status;
                if (status == "InProgress") issue.StartedAt = DateTime.Now;
                if (status == "Resolved") issue.ResolvedAt = DateTime.Now;
                return await Task.FromResult(issue);
            }
            return null;
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
    }
}