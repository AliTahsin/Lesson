using AutoMapper;
using MobileCustomer.API.Models;
using MobileCustomer.API.DTOs;
using MobileCustomer.API.Repositories;

namespace MobileCustomer.API.Services
{
    public class RoomService : IRoomService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RoomService> _logger;

        public RoomService(IMapper mapper, ILogger<RoomService> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<MenuItemDto>> GetMenuAsync()
        {
            var menu = new List<MenuItemDto>();
            var random = new Random();
            
            var categories = new[] { "Appetizer", "Main Course", "Dessert", "Beverage" };
            var items = new[] { "Caesar Salad", "Grilled Salmon", "Beef Steak", "Tiramisu", "Cheesecake", "Cappuccino", "Wine" };
            
            for (int i = 1; i <= 30; i++)
            {
                menu.Add(new MenuItemDto
                {
                    Id = i,
                    Name = items[random.Next(items.Length)] + " " + i,
                    Description = $"Delicious {items[random.Next(items.Length)].ToLower()} prepared by our chefs",
                    Category = categories[random.Next(categories.Length)],
                    Price = random.Next(10, 80),
                    Currency = "EUR",
                    ImageUrl = $"https://picsum.photos/200/150?random={i}",
                    IsAvailable = random.Next(0, 10) > 1,
                    PreparationTimeMinutes = random.Next(10, 45)
                });
            }
            
            return await Task.FromResult(menu);
        }

        public async Task<List<MenuItemDto>> GetMenuByCategoryAsync(string category)
        {
            var allMenu = await GetMenuAsync();
            return allMenu.Where(m => m.Category == category).ToList();
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            return await Task.FromResult(new List<string> { "Appetizer", "Main Course", "Dessert", "Beverage" });
        }

        public async Task<RoomServiceOrderDto> CreateOrderAsync(int userId, CreateRoomServiceOrderDto dto)
        {
            var random = new Random();
            var subTotal = dto.Items.Sum(i => i.Quantity * i.UnitPrice);
            var taxAmount = subTotal * 0.10m;
            var totalAmount = subTotal + taxAmount;
            
            var order = new RoomServiceOrderDto
            {
                Id = random.Next(100, 1000),
                OrderNumber = $"RSO-{DateTime.Now.Ticks}",
                UserId = userId,
                ReservationId = dto.ReservationId,
                RoomNumber = dto.RoomNumber,
                Items = dto.Items,
                SubTotal = subTotal,
                TaxAmount = taxAmount,
                TotalAmount = totalAmount,
                Currency = "EUR",
                SpecialInstructions = dto.SpecialInstructions,
                Status = "Pending",
                OrderTime = DateTime.Now,
                EstimatedDeliveryTime = DateTime.Now.AddMinutes(random.Next(20, 60)),
                CreatedAt = DateTime.Now
            };
            
            return await Task.FromResult(order);
        }

        public async Task<List<RoomServiceOrderDto>> GetUserOrdersAsync(int userId)
        {
            // Mock orders
            var orders = new List<RoomServiceOrderDto>();
            var random = new Random();
            
            for (int i = 1; i <= 5; i++)
            {
                orders.Add(new RoomServiceOrderDto
                {
                    Id = i,
                    OrderNumber = $"RSO-{DateTime.Now.AddDays(-i).Ticks}",
                    UserId = userId,
                    Status = i == 1 ? "Preparing" : (i == 2 ? "OnTheWay" : "Delivered"),
                    TotalAmount = random.Next(50, 500),
                    OrderTime = DateTime.Now.AddMinutes(-random.Next(30, 180))
                });
            }
            
            return await Task.FromResult(orders);
        }

        public async Task<RoomServiceOrderDto> GetOrderByIdAsync(int orderId, int userId)
        {
            var random = new Random();
            return await Task.FromResult(new RoomServiceOrderDto
            {
                Id = orderId,
                OrderNumber = $"RSO-{DateTime.Now.Ticks}",
                UserId = userId,
                Status = "Preparing",
                SubTotal = random.Next(50, 300),
                TaxAmount = random.Next(5, 30),
                TotalAmount = random.Next(60, 350),
                OrderTime = DateTime.Now.AddMinutes(-30),
                EstimatedDeliveryTime = DateTime.Now.AddMinutes(15),
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ItemName = "Caesar Salad", Quantity = 1, UnitPrice = 15, TotalPrice = 15 },
                    new OrderItemDto { ItemName = "Grilled Salmon", Quantity = 1, UnitPrice = 35, TotalPrice = 35 }
                }
            });
        }

        public async Task<bool> CancelOrderAsync(int orderId, int userId)
        {
            return await Task.FromResult(true);
        }

        public async Task<OrderStatusDto> TrackOrderAsync(int orderId, int userId)
        {
            var random = new Random();
            var statuses = new[] { "Pending", "Preparing", "OnTheWay", "Delivered" };
            
            return await Task.FromResult(new OrderStatusDto
            {
                OrderId = orderId,
                Status = statuses[random.Next(0, 3)],
                EstimatedDeliveryTime = DateTime.Now.AddMinutes(random.Next(10, 45)),
                Steps = new List<OrderStepDto>
                {
                    new OrderStepDto { Step = "Order Received", IsCompleted = true, Time = DateTime.Now.AddMinutes(-30) },
                    new OrderStepDto { Step = "Preparing", IsCompleted = random.Next(0, 10) > 5, Time = DateTime.Now.AddMinutes(-15) },
                    new OrderStepDto { Step = "On The Way", IsCompleted = random.Next(0, 10) > 7, Time = null },
                    new OrderStepDto { Step = "Delivered", IsCompleted = false, Time = null }
                }
            });
        }
    }
}