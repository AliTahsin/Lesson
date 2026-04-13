using MobileCustomer.API.Models;
using MobileCustomer.API.Data;

namespace MobileCustomer.API.Repositories
{
    public class DigitalKeyRepository : IDigitalKeyRepository
    {
        private readonly List<DigitalKey> _keys;

        public DigitalKeyRepository()
        {
            _keys = MockData.GetDigitalKeys();
        }

        public async Task<DigitalKey> GetByIdAsync(int id)
        {
            return await Task.FromResult(_keys.FirstOrDefault(k => k.Id == id));
        }

        public async Task<DigitalKey> GetByReservationAsync(int reservationId)
        {
            return await Task.FromResult(_keys.FirstOrDefault(k => k.ReservationId == reservationId));
        }

        public async Task<DigitalKey> GetByKeyCodeAsync(string keyCode)
        {
            return await Task.FromResult(_keys.FirstOrDefault(k => k.KeyCode == keyCode));
        }

        public async Task<DigitalKey> CreateAsync(DigitalKey digitalKey)
        {
            digitalKey.Id = _keys.Max(k => k.Id) + 1;
            digitalKey.CreatedAt = DateTime.Now;
            _keys.Add(digitalKey);
            return await Task.FromResult(digitalKey);
        }

        public async Task<DigitalKey> UpdateAsync(DigitalKey digitalKey)
        {
            var existing = await GetByIdAsync(digitalKey.Id);
            if (existing != null)
            {
                var index = _keys.IndexOf(existing);
                digitalKey.UpdatedAt = DateTime.Now;
                _keys[index] = digitalKey;
            }
            return await Task.FromResult(digitalKey);
        }

        public async Task<bool> ValidateKeyAsync(string keyCode, int roomId)
        {
            var key = await GetByKeyCodeAsync(keyCode);
            if (key == null) return false;
            
            var now = DateTime.Now;
            return key.IsActive && 
                   !key.IsUsed && 
                   key.RoomId == roomId && 
                   key.ValidFrom <= now && 
                   key.ValidUntil >= now;
        }

        public async Task<bool> UseKeyAsync(int id)
        {
            var key = await GetByIdAsync(id);
            if (key != null && key.IsActive && !key.IsUsed)
            {
                key.IsUsed = true;
                key.UsedAt = DateTime.Now;
                key.IsActive = false;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<DigitalKey>> GetActiveKeysByUserAsync(int userId)
        {
            var now = DateTime.Now;
            return await Task.FromResult(_keys
                .Where(k => k.UserId == userId && 
                           k.IsActive && 
                           !k.IsUsed && 
                           k.ValidUntil >= now)
                .ToList());
        }
    }
}