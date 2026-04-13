using MICE.API.Models;
using MICE.API.Data;

namespace MICE.API.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly List<Equipment> _equipment;

        public EquipmentRepository()
        {
            _equipment = MockData.GetEquipment();
        }

        public async Task<Equipment> GetByIdAsync(int id)
        {
            return await Task.FromResult(_equipment.FirstOrDefault(e => e.Id == id));
        }

        public async Task<List<Equipment>> GetAllAsync()
        {
            return await Task.FromResult(_equipment.ToList());
        }

        public async Task<List<Equipment>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_equipment.Where(e => e.HotelId == hotelId).ToList());
        }

        public async Task<List<Equipment>> GetByCategoryAsync(string category)
        {
            return await Task.FromResult(_equipment.Where(e => e.Category == category).ToList());
        }

        public async Task<Equipment> AddAsync(Equipment equipment)
        {
            equipment.Id = _equipment.Max(e => e.Id) + 1;
            equipment.CreatedAt = DateTime.Now;
            _equipment.Add(equipment);
            return await Task.FromResult(equipment);
        }

        public async Task<Equipment> UpdateAsync(Equipment equipment)
        {
            var existing = await GetByIdAsync(equipment.Id);
            if (existing != null)
            {
                var index = _equipment.IndexOf(existing);
                equipment.UpdatedAt = DateTime.Now;
                _equipment[index] = equipment;
            }
            return await Task.FromResult(equipment);
        }

        public async Task<Equipment> UpdateStockAsync(int id, int quantity)
        {
            var equipment = await GetByIdAsync(id);
            if (equipment != null)
            {
                equipment.AvailableQuantity = quantity;
                equipment.UpdatedAt = DateTime.Now;
                await UpdateAsync(equipment);
            }
            return await Task.FromResult(equipment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var equipment = await GetByIdAsync(id);
            if (equipment != null)
            {
                _equipment.Remove(equipment);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> CheckAvailabilityAsync(int equipmentId, int quantity)
        {
            var equipment = await GetByIdAsync(equipmentId);
            return await Task.FromResult(equipment != null && equipment.AvailableQuantity >= quantity);
        }
    }
}