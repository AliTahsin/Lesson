using AutoMapper;
using Restaurant.API.Models;
using Restaurant.API.DTOs;
using Restaurant.API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Restaurant.API.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<StockService> _logger;

        public StockService(
            IStockRepository stockRepository,
            IMapper mapper,
            IHubContext<SignalRHub> hubContext,
            ILogger<StockService> logger)
        {
            _stockRepository = stockRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<StockItemDto> GetStockItemByIdAsync(int id)
        {
            var item = await _stockRepository.GetByIdAsync(id);
            return item != null ? _mapper.Map<StockItemDto>(item) : null;
        }

        public async Task<List<StockItemDto>> GetStockByRestaurantAsync(int restaurantId)
        {
            var items = await _stockRepository.GetByRestaurantAsync(restaurantId);
            return _mapper.Map<List<StockItemDto>>(items);
        }

        public async Task<List<StockItemDto>> GetLowStockItemsAsync(int restaurantId)
        {
            var items = await _stockRepository.GetLowStockItemsAsync(restaurantId);
            return _mapper.Map<List<StockItemDto>>(items);
        }

        public async Task<StockItemDto> AddStockItemAsync(CreateStockItemDto dto)
        {
            var item = _mapper.Map<StockItem>(dto);
            item.IsActive = true;
            item.CreatedAt = DateTime.Now;
            
            await _stockRepository.AddAsync(item);
            return _mapper.Map<StockItemDto>(item);
        }

        public async Task<StockItemDto> UpdateStockItemAsync(int id, UpdateStockItemDto dto)
        {
            var item = await _stockRepository.GetByIdAsync(id);
            if (item == null)
                throw new Exception("Stock item not found");
            
            _mapper.Map(dto, item);
            item.UpdatedAt = DateTime.Now;
            
            await _stockRepository.UpdateAsync(item);
            return _mapper.Map<StockItemDto>(item);
        }

        public async Task<StockItemDto> UpdateStockAsync(int id, int quantity)
        {
            var item = await _stockRepository.UpdateStockAsync(id, quantity);
            return _mapper.Map<StockItemDto>(item);
        }

        public async Task<bool> DeleteStockItemAsync(int id)
        {
            return await _stockRepository.DeleteAsync(id);
        }

        public async Task<StockAlertDto> CheckStockAlertsAsync(int restaurantId)
        {
            var lowStockItems = await _stockRepository.GetLowStockItemsAsync(restaurantId);
            var expiringItems = await _stockRepository.GetExpiringItemsAsync(restaurantId);
            
            var alerts = new List<StockAlertItemDto>();
            
            foreach (var item in lowStockItems)
            {
                alerts.Add(new StockAlertItemDto
                {
                    ItemId = item.Id,
                    ItemName = item.Name,
                    AlertType = "LowStock",
                    CurrentStock = item.CurrentStock,
                    ReorderLevel = item.ReorderLevel,
                    Message = $"Stock level is below reorder level. Current: {item.CurrentStock} {item.Unit}"
                });
            }
            
            foreach (var item in expiringItems)
            {
                alerts.Add(new StockAlertItemDto
                {
                    ItemId = item.Id,
                    ItemName = item.Name,
                    AlertType = "Expiring",
                    ExpiryDate = item.ExpiryDate,
                    Message = $"Item expires on {item.ExpiryDate:dd/MM/yyyy}"
                });
            }
            
            // Send alerts via SignalR if there are critical issues
            if (alerts.Any(a => a.AlertType == "LowStock"))
            {
                await _hubContext.Clients.Group($"restaurant-{restaurantId}-stock").SendAsync("StockAlert", alerts);
            }
            
            return new StockAlertDto
            {
                RestaurantId = restaurantId,
                AlertCount = alerts.Count,
                Alerts = alerts,
                CheckedAt = DateTime.Now
            };
        }
    }
}