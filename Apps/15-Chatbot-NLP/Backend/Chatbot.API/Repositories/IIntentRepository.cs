using Chatbot.API.Models;

namespace Chatbot.API.Repositories
{
    public interface IIntentRepository
    {
        Task<Intent> GetByIdAsync(int id);
        Task<Intent> GetByNameAsync(string name);
        Task<List<Intent>> GetAllAsync();
        Task<Intent> AddAsync(Intent intent);
        Task<Intent> UpdateAsync(Intent intent);
        Task<bool> DeleteAsync(int id);
        Task<List<Intent>> GetActiveIntentsAsync();
    }
}