using Restaurant.API.Models;
using Restaurant.API.Data;

namespace Restaurant.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders;
        private readonly List<OrderItem> _orderItems;

        public OrderRepository()
        {
            _orders = MockData.GetOrders();
            _orderItems = MockData.GetOrderItems();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
            if (order != null)
            {
                order.Items = _orderItems.Where(i => i.OrderId == id).ToList();
            }
            return order;
        }

        public async Task<Order> GetOrderByNumberAsync(string orderNumber)
        {
            var order = await Task.FromResult(_orders.FirstOrDefault(o => o.OrderNumber == orderNumber));
            if (order != null)
            {
                order.Items = _orderItems.Where(i => i.OrderId == order.Id).ToList();
            }
            return order;
        }

        public async Task<List<Order>> GetOrdersByRestaurantAsync(int restaurantId)
        {
            var orders = _orders.Where(o => o.RestaurantId == restaurantId).ToList();
            foreach (var order in orders)
            {
                order.Items = _orderItems.Where(i => i.OrderId == order.Id).ToList();
            }
            return await Task.FromResult(orders);
        }

        public async Task<List<Order>> GetOrdersByTableAsync(int tableId)
        {
            var orders = _orders.Where(o => o.TableId == tableId).ToList();
            foreach (var order in orders)
            {
                order.Items = _orderItems.Where(i => i.OrderId == order.Id).ToList();
            }
            return await Task.FromResult(orders);
        }

        public async Task<List<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            var orders = _orders.Where(o => o.CustomerId == customerId).ToList();
            foreach (var order in orders)
            {
                order.Items = _orderItems.Where(i => i.OrderId == order.Id).ToList();
            }
            return await Task.FromResult(orders);
        }

        public async Task<List<Order>> GetOrdersByRoomAsync(int roomId)
        {
            var orders = _orders.Where(o => o.RoomId == roomId).ToList();
            foreach (var order in orders)
            {
                order.Items = _orderItems.Where(i => i.OrderId == order.Id).ToList();
            }
            return await Task.FromResult(orders);
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(string status)
        {
            var orders = _orders.Where(o => o.Status == status).ToList();
            foreach (var order in orders)
            {
                order.Items = _orderItems.Where(i => i.OrderId == order.Id).ToList();
            }
            return await Task.FromResult(orders);
        }

        public async Task<List<Order>> GetPendingOrdersAsync(int restaurantId)
        {
            var statuses = new[] { "Pending", "Preparing" };
            var orders = _orders.Where(o => o.RestaurantId == restaurantId && statuses.Contains(o.Status)).ToList();
            foreach (var order in orders)
            {
                order.Items = _orderItems.Where(i => i.OrderId == order.Id).ToList();
            }
            return await Task.FromResult(orders);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.Id = _orders.Max(o => o.Id) + 1;
            order.OrderNumber = $"ORD-{DateTime.Now.Ticks}-{order.Id}";
            order.CreatedAt = DateTime.Now;
            _orders.Add(order);
            
            foreach (var item in order.Items)
            {
                item.Id = _orderItems.Max(i => i.Id) + 1;
                item.OrderId = order.Id;
                _orderItems.Add(item);
            }
            
            return await Task.FromResult(order);
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            var existing = await GetOrderByIdAsync(order.Id);
            if (existing != null)
            {
                var index = _orders.IndexOf(existing);
                order.UpdatedAt = DateTime.Now;
                _orders[index] = order;
            }
            return await Task.FromResult(order);
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                order.UpdatedAt = DateTime.Now;
                
                switch (status)
                {
                    case "Preparing":
                        order.PreparationStartTime = DateTime.Now;
                        break;
                    case "Ready":
                        order.ReadyTime = DateTime.Now;
                        break;
                    case "Served":
                        order.ServedTime = DateTime.Now;
                        break;
                    case "Completed":
                        order.CompletedTime = DateTime.Now;
                        break;
                }
                
                var index = _orders.IndexOf(order);
                _orders[index] = order;
            }
            return await Task.FromResult(order);
        }

        public async Task<OrderItem> UpdateOrderItemStatusAsync(int orderItemId, string status)
        {
            var item = _orderItems.FirstOrDefault(i => i.Id == orderItemId);
            if (item != null)
            {
                item.Status = status;
                var index = _orderItems.IndexOf(item);
                _orderItems[index] = item;
            }
            return await Task.FromResult(item);
        }

        public async Task<decimal> GetDailyRevenueAsync(int restaurantId, DateTime date)
        {
            var completedOrders = _orders.Where(o => 
                o.RestaurantId == restaurantId && 
                o.Status == "Completed" &&
                o.CompletedTime.HasValue &&
                o.CompletedTime.Value.Date == date.Date);
            
            return await Task.FromResult(completedOrders.Sum(o => o.TotalAmount));
        }

        public async Task<Dictionary<string, int>> GetPopularItemsAsync(int restaurantId, int days = 7)
        {
            var startDate = DateTime.Now.AddDays(-days);
            var recentOrderItems = from order in _orders
                                   where order.RestaurantId == restaurantId && 
                                         order.CreatedAt >= startDate &&
                                         order.Status == "Completed"
                                   join item in _orderItems on order.Id equals item.OrderId
                                   select item;
            
            return await Task.FromResult(recentOrderItems
                .GroupBy(i => i.ItemName)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity)));
        }
    }
}