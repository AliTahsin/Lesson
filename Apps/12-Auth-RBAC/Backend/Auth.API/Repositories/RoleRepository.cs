using Auth.API.Models;
using Auth.API.Data;

namespace Auth.API.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly List<Role> _roles;
        private readonly List<Permission> _permissions;

        public RoleRepository()
        {
            _roles = MockData.GetRoles();
            _permissions = MockData.GetPermissions();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await Task.FromResult(_roles.FirstOrDefault(r => r.Id == id));
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            return await Task.FromResult(_roles.FirstOrDefault(r => r.Name == name));
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await Task.FromResult(_roles.ToList());
        }

        public async Task<List<Role>> GetByLevelAsync(string level)
        {
            return await Task.FromResult(_roles.Where(r => r.Level == level).ToList());
        }

        public async Task<Role> AddAsync(Role role)
        {
            role.Id = _roles.Max(r => r.Id) + 1;
            role.CreatedAt = DateTime.Now;
            _roles.Add(role);
            return await Task.FromResult(role);
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            var existing = await GetByIdAsync(role.Id);
            if (existing != null)
            {
                var index = _roles.IndexOf(existing);
                role.UpdatedAt = DateTime.Now;
                _roles[index] = role;
            }
            return await Task.FromResult(role);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await GetByIdAsync(id);
            if (role != null)
            {
                _roles.Remove(role);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> AddPermissionToRoleAsync(int roleId, int permissionId)
        {
            var role = await GetByIdAsync(roleId);
            if (role != null)
            {
                if (role.PermissionIds == null)
                    role.PermissionIds = new List<int>();
                if (!role.PermissionIds.Contains(permissionId))
                    role.PermissionIds.Add(permissionId);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> RemovePermissionFromRoleAsync(int roleId, int permissionId)
        {
            var role = await GetByIdAsync(roleId);
            if (role != null && role.PermissionIds != null)
            {
                role.PermissionIds.Remove(permissionId);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<Permission>> GetRolePermissionsAsync(int roleId)
        {
            var role = await GetByIdAsync(roleId);
            if (role != null && role.PermissionIds != null)
            {
                return await Task.FromResult(_permissions.Where(p => role.PermissionIds.Contains(p.Id)).ToList());
            }
            return await Task.FromResult(new List<Permission>());
        }
    }
}