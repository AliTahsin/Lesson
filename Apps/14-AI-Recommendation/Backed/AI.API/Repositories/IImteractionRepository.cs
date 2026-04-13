using AI.API.Models;

namespace AI.API.Repositories
{
    public interface IInteractionRepository
    {
        Task<UserInteraction> GetByIdAsync(int id);
        Task<List<UserInteraction>> GetByUserAsync(int userId);
        Task<List<UserInteraction>> GetByItemAsync(int itemId, string itemType);
        Task<List<UserInteraction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<UserInteraction> AddAsync(UserInteraction interaction);
        Task<bool> DeleteAsync(int id);
        Task<List<UserInteraction>> GetUserBehaviorAsync(int userId, int days = 30);
        Task<Dictionary<int, decimal>> GetItemRatingsAsync(string itemType);
    }
}