using AutoMapper;
using Restaurant.API.Models;
using Restaurant.API.DTOs;
using Restaurant.API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Restaurant.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IMenuRepository menuRepository,
            IMapper mapper,
            IHubContext<SignalRHub> hubContext,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _menuRepository = menuRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            return order != null ? _mapper.Map<OrderDto>(order) : null;
        }

        public async Task<OrderDto> GetOrderByNumberAsync(string orderNumber)
        {
            var order = await _orderRepository.GetOrderByNumberAsync(orderNumber);
            return order != null ? _mapper.Map<OrderDto>(order) : null;
        }

        public async Task<List<OrderDto>> GetOrdersByRestaurantAsync(int restaurantId)
        {
            var orders = await _orderRepository.GetOrdersByRestaurantAsync(restaurantId);
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<List<OrderDto>> GetPendingOrdersAsync(int restaurantId)
        {
            var orders = await _orderRepository.GetPendingOrdersAsync(restaurantId);
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            // Calculate totals
            decimal subTotal = 0;
            var orderItems = new List<OrderItem>();
            
            foreach (var itemDto in dto.Items)
            {
                var menuItem = await _menuRepository.GetMenuItemByIdAsync(itemDto.MenuItemId);
                if (menuItem == null)
                    throw new Exception($"Menu item {itemDto.MenuItemId} not found");
                
                if (!menuItem.IsAvailable)
                    throw new Exception($"Menu item {menuItem.Name} is not available");
                
                var totalPrice = menuItem.Price * itemDto.Quantity;
                subTotal += totalPrice;
                
                orderItems.Add(new OrderItem
                {
                    MenuItemId = itemDto.MenuItemId,
                    ItemName = menuItem.Name,
                    Quantity = itemDto.Quantity,
                    UnitPrice = menuItem.Price,
                    TotalPrice = totalPrice,
                    SpecialInstructions = itemDto.SpecialInstructions,
                    Status = "Pending"
                });
            }
            
            var taxAmount = subTotal * 0.10m; // 10% tax
            var totalAmount = subTotal + taxAmount;
            
            var order = new Order
            {
                RestaurantId = dto.RestaurantId,
                TableId = dto.TableId,
                TableNumber = dto.TableNumber,
                CustomerId = dto.CustomerId,
                CustomerName = dto.CustomerName,
                RoomId = dto.RoomId,
                RoomNumber = dto.RoomNumber,
                OrderType = dto.OrderType,
                Status = "Pending",
                SubTotal = subTotal,
                TaxAmount = taxAmount,
                TaxRate = 10,
                TotalAmount = totalAmount,
                Currency = "EUR",
                SpecialInstructions = dto.SpecialInstructions,
                OrderTime = DateTime.Now,
                Items = orderItems,
                CreatedAt = DateTime.Now
            };
            
            var createdOrder = await _orderRepository.CreateOrderAsync(order);
            
            // Send real-time notification via SignalR
            await _hubContext.Clients.Group($"restaurant-{dto.RestaurantId}").SendAsync("NewOrder", createdOrder);
            
            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _orderRepository.UpdateOrderStatusAsync(orderId, status);
            
            // Send real-time notification
            await _hubContext.Clients.Group($"order-{orderId}").SendAsync("OrderStatusUpdated", orderId, status);
            await _hubContext.Clients.Group($"restaurant-{order.RestaurantId}").SendAsync("OrderUpdated", order);
            
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderItemStatusAsync(int orderItemId, string status)
        {
            var orderItem = await _orderRepository.UpdateOrderItemStatusAsync(orderItemId, status);
            var order = await _orderRepository.GetOrderByIdAsync(orderItem.OrderId);
            
            // Check if all items are ready
            if (order.Items.All(i => i.Status == "Ready"))
            {
                await UpdateOrderStatusAsync(order.Id, "Ready");
            }
            
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CancelOrderAsync(int orderId, string reason)
        {
            var order = await _orderRepository.UpdateOrderStatusAsync(orderId, "Cancelled");
            
            // Send real-time notification
            await _hubContext.Clients.Group($"order-{orderId}").SendAsync("OrderCancelled", orderId, reason);
            
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<DailyRevenueDto> GetDailyRevenueAsync(int restaurantId, DateTime date)
        {
            var revenue = await _orderRepository.GetDailyRevenueAsync(restaurantId, date);
            var orderCount = (await _orderRepository.GetOrdersByRestaurantAsync(restaurantId))
                .Count(o => o.CompletedTime.HasValue && o.CompletedTime.Value.Date == date.Date);
            
            return new DailyRevenueDto
            {
                Date = date,
                Revenue = revenue,
                OrderCount = orderCount,
                AverageOrderValue = orderCount > 0 ? revenue / orderCount : 0
            };
        }

        public async Task<PopularItemsDto> GetPopularItemsAsync(int restaurantId, int days = 7)
        {
            var popularItems = await _orderRepository.GetPopularItemsAsync(restaurantId, days);
            
            return new PopularItemsDto
            {
                Period = $"{days} days",
                Items = popularItems.Select(kv => new PopularItemDto
                {
                    Name = kv.Key,
                    Quantity = kv.Value
                }).OrderByDescending(i => i.Quantity).Take(10).ToList()
            };
        }
    }
}