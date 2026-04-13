using Chatbot.API.Models;
using Chatbot.API.Data;

namespace Chatbot.API.Repositories
{
    public class IntentRepository : IIntentRepository
    {
        private readonly List<Intent> _intents;

        public IntentRepository()
        {
            _intents = MockData.GetIntents();
        }

        public async Task<Intent> GetByIdAsync(int id)
        {
            return await Task.FromResult(_intents.FirstOrDefault(i => i.Id == id));
        }

        public async Task<Intent> GetByNameAsync(string name)
        {
            return await Task.FromResult(_intents.FirstOrDefault(i => i.Name == name));
        }

        public async Task<List<Intent>> GetAllAsync()
        {
            return await Task.FromResult(_intents.ToList());
        }

        public async Task<Intent> AddAsync(Intent intent)
        {
            intent.Id = _intents.Max(i => i.Id) + 1;
            intent.CreatedAt = DateTime.Now;
            _intents.Add(intent);
            return await Task.FromResult(intent);
        }

        public async Task<Intent> UpdateAsync(Intent intent)
        {
            var existing = await GetByIdAsync(intent.Id);
            if (existing != null)
            {
                var index = _intents.IndexOf(existing);
                intent.UpdatedAt = DateTime.Now;
                _intents[index] = intent;
            }
            return await Task.FromResult(intent);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var intent = await GetByIdAsync(id);
            if (intent != null)
            {
                _intents.Remove(intent);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<Intent>> GetActiveIntentsAsync()
        {
            return await Task.FromResult(_intents.Where(i => i.IsActive).ToList());
        }
    }
}