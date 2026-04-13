using Auth.API.Models;
using Auth.API.Data;

namespace Auth.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public UserRepository()
        {
            _users = MockData.GetUsers();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await Task.FromResult(_users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower()));
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await Task.FromResult(_users.FirstOrDefault(u => u.Username == username));
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await Task.FromResult(_users.ToList());
        }

        public async Task<List<User>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_users.Where(u => u.HotelId == hotelId).ToList());
        }

        public async Task<List<User>> GetByRoleAsync(int roleId)
        {
            return await Task.FromResult(_users.Where(u => u.RoleIds.Contains(roleId)).ToList());
        }

        public async Task<User> AddAsync(User user)
        {
            user.Id = _users.Max(u => u.Id) + 1;
            user.CreatedAt = DateTime.Now;
            _users.Add(user);
            return await Task.FromResult(user);
        }

        public async Task<User> UpdateAsync(User user)
        {
            var existing = await GetByIdAsync(user.Id);
            if (existing != null)
            {
                var index = _users.IndexOf(existing);
                user.UpdatedAt = DateTime.Now;
                _users[index] = user;
            }
            return await Task.FromResult(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _users.Remove(user);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.LastLoginAt = DateTime.Now;
                user.LastActivityAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateFailedLoginAttemptsAsync(int userId, int attempts)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.FailedLoginAttempts = attempts;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> LockUserAsync(int userId, DateTime lockoutEnd)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnd = lockoutEnd;
                user.IsActive = false;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> UnlockUserAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnd = null;
                user.IsActive = true;
                user.FailedLoginAttempts = 0;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPasswordHash)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.PasswordHash = newPasswordHash;
                user.PasswordChangedAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> VerifyEmailAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.IsEmailVerified = true;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> VerifyPhoneAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.IsPhoneVerified = true;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateTwoFactorSettingsAsync(int userId, bool enabled, string method)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.TwoFactorEnabled = enabled;
                user.TwoFactorMethod = method;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}