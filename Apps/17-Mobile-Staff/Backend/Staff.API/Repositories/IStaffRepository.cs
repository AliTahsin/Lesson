using Staff.API.Models;

namespace Staff.API.Repositories
{
    public interface IStaffRepository
    {
        Task<Staff> GetByIdAsync(int id);
        Task<Staff> GetByEmailAsync(string email);
        Task<List<Staff>> GetAllAsync();
        Task<List<Staff>> GetByHotelAsync(int hotelId);
        Task<List<Staff>> GetByRoleAsync(string role);
        Task<Staff> AddAsync(Staff staff);
        Task<Staff> UpdateAsync(Staff staff);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateLastLoginAsync(int id);
    }
}