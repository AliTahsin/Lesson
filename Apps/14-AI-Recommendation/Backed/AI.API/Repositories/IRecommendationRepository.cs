using AI.API.Models;

namespace AI.API.Repositories
{
    public interface IRecommendationRepository
    {
        Task<Recommendation> GetByIdAsync(int id);
        Task<List<Recommendation>> GetByUserAsync(int userId);
        Task<List<Recommendation>> GetByItemAsync(int itemId);
        Task<Recommendation> AddAsync(Recommendation recommendation);
        Task<Recommendation> UpdateAsync(Recommendation recommendation);
        Task<bool> MarkAsClickedAsync(int id);
        Task<bool> MarkAsBookedAsync(int id);
        Task<List<Recommendation>> GetRecommendationsForUserAsync(int userId, int count = 10);
        Task<List<Recommendation>> GetTopRecommendationsAsync(string itemType, int count = 10);
    }
}