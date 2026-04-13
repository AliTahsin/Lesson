using AutoMapper;
using Restaurant.API.Models;
using Restaurant.API.DTOs;
using Restaurant.API.Repositories;

namespace Restaurant.API.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(
            IRestaurantRepository restaurantRepository,
            IMapper mapper,
            ILogger<RestaurantService> logger)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RestaurantDto> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
            return restaurant != null ? _mapper.Map<RestaurantDto>(restaurant) : null;
        }

        public async Task<List<RestaurantDto>> GetRestaurantsByHotelAsync(int hotelId)
        {
            var restaurants = await _restaurantRepository.GetByHotelAsync(hotelId);
            return _mapper.Map<List<RestaurantDto>>(restaurants);
        }

        public async Task<RestaurantDto> CreateRestaurantAsync(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.IsActive = true;
            restaurant.CreatedAt = DateTime.Now;
            
            await _restaurantRepository.AddAsync(restaurant);
            return _mapper.Map<RestaurantDto>(restaurant);
        }

        public async Task<RestaurantDto> UpdateRestaurantAsync(int id, UpdateRestaurantDto dto)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
            if (restaurant == null)
                throw new Exception("Restaurant not found");
            
            _mapper.Map(dto, restaurant);
            restaurant.UpdatedAt = DateTime.Now;
            
            await _restaurantRepository.UpdateAsync(restaurant);
            return _mapper.Map<RestaurantDto>(restaurant);
        }

        public async Task<bool> DeleteRestaurantAsync(int id)
        {
            return await _restaurantRepository.DeleteAsync(id);
        }

        public async Task<List<TableDto>> GetTablesByRestaurantAsync(int restaurantId)
        {
            var tables = await _restaurantRepository.GetTablesByRestaurantAsync(restaurantId);
            return _mapper.Map<List<TableDto>>(tables);
        }

        public async Task<TableDto> UpdateTableStatusAsync(int tableId, string status)
        {
            var table = await _restaurantRepository.UpdateTableStatusAsync(tableId, status);
            return _mapper.Map<TableDto>(table);
        }

        public async Task<string> GenerateTableQrCodeAsync(int tableId)
        {
            var table = await _restaurantRepository.GetTableByIdAsync(tableId);
            if (table == null)
                throw new Exception("Table not found");
            
            // Generate QR code URL
            var qrUrl = $"https://order.hotelapp.com/table/{tableId}";
            table.QrCodeUrl = qrUrl;
            
            await _restaurantRepository.UpdateTableStatusAsync(tableId, table.Status);
            return qrUrl;
        }
    }
}