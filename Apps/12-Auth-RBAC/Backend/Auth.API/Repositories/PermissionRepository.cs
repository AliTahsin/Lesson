using Auth.API.Models;
using Auth.API.Data;

namespace Auth.API.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly List<Permission> _permissions;

        public PermissionRepository()
        {
            _permissions = MockData.GetPermissions();
        }

        public async Task<Permission> GetByIdAsync(int id)
        {
            return await Task.FromResult(_permissions.FirstOrDefault(p => p.Id == id));
        }

        public async Task<Permission> GetByCodeAsync(string code)
        {
            return await Task.FromResult(_permissions.FirstOrDefault(p => p.Code == code));
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            return await Task.FromResult(_permissions.ToList());
        }

        public async Task<List<Permission>> GetByCategoryAsync(string category)
        {
            return await Task.FromResult(_permissions.Where(p => p.Category == category).ToList());
        }

        public async Task<Permission> AddAsync(Permission permission)
        {
            permission.Id = _permissions.Max(p => p.Id) + 1;
            permission.CreatedAt = DateTime.Now;
            _permissions.Add(permission);
            return await Task.FromResult(permission);
        }

        public async Task<Permission> UpdateAsync(Permission permission)
        {
            var existing = await GetByIdAsync(permission.Id);
            if (existing != null)
            {
                var index = _permissions.IndexOf(existing);
                _permissions[index] = permission;
            }
            return await Task.FromResult(permission);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var permission = await GetByIdAsync(id);
            if (permission != null)
            {
                _permissions.Remove(permission);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}