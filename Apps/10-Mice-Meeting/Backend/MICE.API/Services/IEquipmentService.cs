using MICE.API.DTOs;

namespace MICE.API.Services
{
    public interface IEquipmentService
    {
        Task<EquipmentDto> GetEquipmentByIdAsync(int id);
        Task<List<EquipmentDto>> GetEquipmentByHotelAsync(int hotelId);
        Task<List<EquipmentDto>> GetEquipmentByCategoryAsync(string category);
        Task<EquipmentDto> CreateEquipmentAsync(CreateEquipmentDto dto);
        Task<EquipmentDto> UpdateEquipmentAsync(int id, UpdateEquipmentDto dto);
        Task<EquipmentDto> UpdateStockAsync(int id, int quantity);
        Task<bool> DeleteEquipmentAsync(int id);
    }
}