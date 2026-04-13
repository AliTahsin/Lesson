using Reporting.API.Models;
using Reporting.API.Data;

namespace Reporting.API.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly List<Dashboard> _dashboards;

        public DashboardRepository()
        {
            _dashboards = MockData.GetDashboards();
        }

        public async Task<Dashboard> GetByIdAsync(int id)
        {
            return await Task.FromResult(_dashboards.FirstOrDefault(d => d.Id == id));
        }

        public async Task<List<Dashboard>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_dashboards.Where(d => d.HotelId == hotelId).ToList());
        }

        public async Task<Dashboard> GetDefaultDashboardAsync(int hotelId, string dashboardType)
        {
            return await Task.FromResult(_dashboards.FirstOrDefault(d => 
                d.HotelId == hotelId && d.DashboardType == dashboardType && d.IsDefault));
        }

        public async Task<Dashboard> AddAsync(Dashboard dashboard)
        {
            dashboard.Id = _dashboards.Max(d => d.Id) + 1;
            dashboard.CreatedAt = DateTime.Now;
            _dashboards.Add(dashboard);
            return await Task.FromResult(dashboard);
        }

        public async Task<Dashboard> UpdateAsync(Dashboard dashboard)
        {
            var existing = await GetByIdAsync(dashboard.Id);
            if (existing != null)
            {
                var index = _dashboards.IndexOf(existing);
                dashboard.UpdatedAt = DateTime.Now;
                _dashboards[index] = dashboard;
            }
            return await Task.FromResult(dashboard);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dashboard = await GetByIdAsync(id);
            if (dashboard != null)
            {
                _dashboards.Remove(dashboard);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}