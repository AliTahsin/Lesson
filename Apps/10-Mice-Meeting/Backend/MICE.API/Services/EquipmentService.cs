using AutoMapper;
using MICE.API.Models;
using MICE.API.DTOs;
using MICE.API.Repositories;

namespace MICE.API.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EquipmentService> _logger;

        public EquipmentService(
            IEquipmentRepository equipmentRepository,
            IMapper mapper,
            ILogger<EquipmentService> logger)
        {
            _equipmentRepository = equipmentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EquipmentDto> GetEquipmentByIdAsync(int id)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(id);
            return equipment != null ? _mapper.Map<EquipmentDto>(equipment) : null;
        }

        public async Task<List<EquipmentDto>> GetEquipmentByHotelAsync(int hotelId)
        {
            var equipment = await _equipmentRepository.GetByHotelAsync(hotelId);
            return _mapper.Map<List<EquipmentDto>>(equipment);
        }

        public async Task<List<EquipmentDto>> GetEquipmentByCategoryAsync(string category)
        {
            var equipment = await _equipmentRepository.GetByCategoryAsync(category);
            return _mapper.Map<List<EquipmentDto>>(equipment);
        }

        public async Task<EquipmentDto> CreateEquipmentAsync(CreateEquipmentDto dto)
        {
            var equipment = _mapper.Map<Equipment>(dto);
            equipment.AvailableQuantity = equipment.TotalQuantity;
            equipment.IsActive = true;
            equipment.CreatedAt = DateTime.Now;
            
            await _equipmentRepository.AddAsync(equipment);
            return _mapper.Map<EquipmentDto>(equipment);
        }

        public async Task<EquipmentDto> UpdateEquipmentAsync(int id, UpdateEquipmentDto dto)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(id);
            if (equipment == null)
                throw new Exception("Equipment not found");
            
            _mapper.Map(dto, equipment);
            equipment.UpdatedAt = DateTime.Now;
            
            await _equipmentRepository.UpdateAsync(equipment);
            return _mapper.Map<EquipmentDto>(equipment);
        }

        public async Task<EquipmentDto> UpdateStockAsync(int id, int quantity)
        {
            var equipment = await _equipmentRepository.UpdateStockAsync(id, quantity);
            return _mapper.Map<EquipmentDto>(equipment);
        }

        public async Task<bool> DeleteEquipmentAsync(int id)
        {
            return await _equipmentRepository.DeleteAsync(id);
        }
    }
}