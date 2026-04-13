using Staff.API.Models;
using Staff.API.Data;

namespace Staff.API.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly List<Staff> _staffs;

        public StaffRepository()
        {
            _staffs = MockData.GetStaffs();
        }

        public async Task<Staff> GetByIdAsync(int id)
        {
            return await Task.FromResult(_staffs.FirstOrDefault(s => s.Id == id));
        }

        public async Task<Staff> GetByEmailAsync(string email)
        {
            return await Task.FromResult(_staffs.FirstOrDefault(s => s.Email.ToLower() == email.ToLower()));
        }

        public async Task<List<Staff>> GetAllAsync()
        {
            return await Task.FromResult(_staffs.ToList());
        }

        public async Task<List<Staff>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_staffs.Where(s => s.HotelId == hotelId).ToList());
        }

        public async Task<List<Staff>> GetByRoleAsync(string role)
        {
            return await Task.FromResult(_staffs.Where(s => s.Role == role).ToList());
        }

        public async Task<Staff> AddAsync(Staff staff)
        {
            staff.Id = _staffs.Max(s => s.Id) + 1;
            staff.CreatedAt = DateTime.Now;
            _staffs.Add(staff);
            return await Task.FromResult(staff);
        }

        public async Task<Staff> UpdateAsync(Staff staff)
        {
            var existing = await GetByIdAsync(staff.Id);
            if (existing != null)
            {
                var index = _staffs.IndexOf(existing);
                staff.UpdatedAt = DateTime.Now;
                _staffs[index] = staff;
            }
            return await Task.FromResult(staff);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var staff = await GetByIdAsync(id);
            if (staff != null)
            {
                _staffs.Remove(staff);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateLastLoginAsync(int id)
        {
            var staff = await GetByIdAsync(id);
            if (staff != null)
            {
                staff.LastLoginAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}