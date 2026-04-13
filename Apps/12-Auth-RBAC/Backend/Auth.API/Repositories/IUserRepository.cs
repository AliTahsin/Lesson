using Auth.API.Models;

namespace Auth.API.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<List<User>> GetAllAsync();
        Task<List<User>> GetByHotelAsync(int hotelId);
        Task<List<User>> GetByRoleAsync(int roleId);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateLastLoginAsync(int userId);
        Task<bool> UpdateFailedLoginAttemptsAsync(int userId, int attempts);
        Task<bool> LockUserAsync(int userId, DateTime lockoutEnd);
        Task<bool> UnlockUserAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string newPasswordHash);
        Task<bool> VerifyEmailAsync(int userId);
        Task<bool> VerifyPhoneAsync(int userId);
        Task<bool> UpdateTwoFactorSettingsAsync(int userId, bool enabled, string method);
    }
}