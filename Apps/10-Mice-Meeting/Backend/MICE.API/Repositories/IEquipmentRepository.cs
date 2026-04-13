using MICE.API.Models;

namespace MICE.API.Repositories
{
    public interface IEquipmentRepository
    {
        Task<Equipment> GetByIdAsync(int id);
        Task<List<Equipment>> GetAllAsync();
        Task<List<Equipment>> GetByHotelAsync(int hotelId);
        Task<List<Equipment>> GetByCategoryAsync(string category);
        Task<Equipment> AddAsync(Equipment equipment);
        Task<Equipment> UpdateAsync(Equipment equipment);
        Task<Equipment> UpdateStockAsync(int id, int quantity);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckAvailabilityAsync(int equipmentId, int quantity);
    }
}