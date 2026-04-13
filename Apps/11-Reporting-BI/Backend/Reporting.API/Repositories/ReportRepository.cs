using Reporting.API.Models;
using Reporting.API.Data;

namespace Reporting.API.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly List<Report> _reports;

        public ReportRepository()
        {
            _reports = MockData.GetReports();
        }

        public async Task<Report> GetByIdAsync(int id)
        {
            return await Task.FromResult(_reports.FirstOrDefault(r => r.Id == id));
        }

        public async Task<List<Report>> GetAllAsync()
        {
            return await Task.FromResult(_reports.ToList());
        }

        public async Task<List<Report>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_reports.Where(r => r.HotelId == hotelId).ToList());
        }

        public async Task<List<Report>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(_reports.Where(r => r.GeneratedAt >= startDate && r.GeneratedAt <= endDate).ToList());
        }

        public async Task<Report> AddAsync(Report report)
        {
            report.Id = _reports.Max(r => r.Id) + 1;
            report.CreatedAt = DateTime.Now;
            _reports.Add(report);
            return await Task.FromResult(report);
        }

        public async Task<Report> UpdateAsync(Report report)
        {
            var existing = await GetByIdAsync(report.Id);
            if (existing != null)
            {
                var index = _reports.IndexOf(existing);
                _reports[index] = report;
            }
            return await Task.FromResult(report);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var report = await GetByIdAsync(id);
            if (report != null)
            {
                _reports.Remove(report);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}