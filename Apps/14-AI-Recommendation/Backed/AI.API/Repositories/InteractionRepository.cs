using AI.API.Models;
using AI.API.Data;

namespace AI.API.Repositories
{
    public class InteractionRepository : IInteractionRepository
    {
        private readonly List<UserInteraction> _interactions;

        public InteractionRepository()
        {
            _interactions = MockData.GetInteractions();
        }

        public async Task<UserInteraction> GetByIdAsync(int id)
        {
            return await Task.FromResult(_interactions.FirstOrDefault(i => i.Id == id));
        }

        public async Task<List<UserInteraction>> GetByUserAsync(int userId)
        {
            return await Task.FromResult(_interactions.Where(i => i.UserId == userId).ToList());
        }

        public async Task<List<UserInteraction>> GetByItemAsync(int itemId, string itemType)
        {
            return await Task.FromResult(_interactions
                .Where(i => i.ItemId == itemId && i.ItemType == itemType)
                .ToList());
        }

        public async Task<List<UserInteraction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(_interactions
                .Where(i => i.InteractionDate >= startDate && i.InteractionDate <= endDate)
                .ToList());
        }

        public async Task<UserInteraction> AddAsync(UserInteraction interaction)
        {
            interaction.Id = _interactions.Max(i => i.Id) + 1;
            interaction.InteractionDate = DateTime.Now;
            _interactions.Add(interaction);
            return await Task.FromResult(interaction);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var interaction = await GetByIdAsync(id);
            if (interaction != null)
            {
                _interactions.Remove(interaction);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<UserInteraction>> GetUserBehaviorAsync(int userId, int days = 30)
        {
            var cutoff = DateTime.Now.AddDays(-days);
            return await Task.FromResult(_interactions
                .Where(i => i.UserId == userId && i.InteractionDate >= cutoff)
                .OrderByDescending(i => i.InteractionDate)
                .ToList());
        }

        public async Task<Dictionary<int, decimal>> GetItemRatingsAsync(string itemType)
        {
            var ratings = _interactions
                .Where(i => i.ItemType == itemType && i.Rating.HasValue)
                .GroupBy(i => i.ItemId)
                .ToDictionary(g => g.Key, g => g.Average(i => i.Rating.Value));
            
            return await Task.FromResult(ratings);
        }
    }
}