using AI.API.Models;
using AI.API.Data;

namespace AI.API.Repositories
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly List<Recommendation> _recommendations;

        public RecommendationRepository()
        {
            _recommendations = MockData.GetRecommendations();
        }

        public async Task<Recommendation> GetByIdAsync(int id)
        {
            return await Task.FromResult(_recommendations.FirstOrDefault(r => r.Id == id));
        }

        public async Task<List<Recommendation>> GetByUserAsync(int userId)
        {
            return await Task.FromResult(_recommendations.Where(r => r.UserId == userId).ToList());
        }

        public async Task<List<Recommendation>> GetByItemAsync(int itemId)
        {
            return await Task.FromResult(_recommendations.Where(r => r.ItemId == itemId).ToList());
        }

        public async Task<Recommendation> AddAsync(Recommendation recommendation)
        {
            recommendation.Id = _recommendations.Max(r => r.Id) + 1;
            recommendation.RecommendedAt = DateTime.Now;
            _recommendations.Add(recommendation);
            return await Task.FromResult(recommendation);
        }

        public async Task<Recommendation> UpdateAsync(Recommendation recommendation)
        {
            var existing = await GetByIdAsync(recommendation.Id);
            if (existing != null)
            {
                var index = _recommendations.IndexOf(existing);
                _recommendations[index] = recommendation;
            }
            return await Task.FromResult(recommendation);
        }

        public async Task<bool> MarkAsClickedAsync(int id)
        {
            var recommendation = await GetByIdAsync(id);
            if (recommendation != null)
            {
                recommendation.IsClicked = true;
                recommendation.ClickedAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> MarkAsBookedAsync(int id)
        {
            var recommendation = await GetByIdAsync(id);
            if (recommendation != null)
            {
                recommendation.IsBooked = true;
                recommendation.BookedAt = DateTime.Now;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<Recommendation>> GetRecommendationsForUserAsync(int userId, int count = 10)
        {
            return await Task.FromResult(_recommendations
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Score)
                .Take(count)
                .ToList());
        }

        public async Task<List<Recommendation>> GetTopRecommendationsAsync(string itemType, int count = 10)
        {
            return await Task.FromResult(_recommendations
                .Where(r => r.ItemType == itemType)
                .OrderByDescending(r => r.Score)
                .Take(count)
                .ToList());
        }
    }
}