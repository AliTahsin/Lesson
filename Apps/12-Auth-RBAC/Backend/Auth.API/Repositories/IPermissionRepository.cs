using Auth.API.Models;

namespace Auth.API.Repositories
{
    public interface IPermissionRepository
    {
        Task<Permission> GetByIdAsync(int id);
        Task<Permission> GetByCodeAsync(string code);
        Task<List<Permission>> GetAllAsync();
        Task<List<Permission>> GetByCategoryAsync(string category);
        Task<Permission> AddAsync(Permission permission);
        Task<Permission> UpdateAsync(Permission permission);
        Task<bool> DeleteAsync(int id);
    }
}