using Restaurant.API.Models;

namespace Restaurant.API.Data
{
    public static class MockData
    {
        private static readonly Random _random = new();

        public static List<Restaurant> GetRestaurants()
        {
            return new List<Restaurant>
            {
                new Restaurant
                {
                    Id = 1,
                    Name = "Main Restaurant",
                    HotelId = 1,
                    CuisineType = "International",
                    Description = "Main restaurant serving breakfast, lunch and dinner",
                    OpeningTime = "06:00",
                    ClosingTime = "23:00",
                    TotalTables = 50,
                    TotalCapacity = 200,
                    AveragePricePerPerson = 35,
                    IsActive = true,
                    IsBreakfastAvailable = true,
                    IsLunchAvailable = true,
                    IsDinnerAvailable = true,
                    Images = new List<string> { "rest1_1.jpg", "rest1_2.jpg" },
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new Restaurant
                {
                    Id = 2,
                    Name = "Italian Restaurant",
                    HotelId = 1,
                    CuisineType = "Italian",
                    Description = "Authentic Italian cuisine",
                    OpeningTime = "12:00",
                    ClosingTime = "22:00",
                    TotalTables = 30,
                    TotalCapacity = 120,
                    AveragePricePerPerson = 45,
                    IsActive = true,
                    IsBreakfastAvailable = false,
                    IsLunchAvailable = true,
                    IsDinnerAvailable = true,
                    Images = new List<string> { "rest2_1.jpg" },
                    CreatedAt = DateTime.Now.AddMonths(-4)
                },
                new Restaurant
                {
                    Id = 3,
                    Name = "Sushi Bar",
                    HotelId = 2,
                    CuisineType = "Japanese",
                    Description = "Fresh sushi and Japanese cuisine",
                    OpeningTime = "18:00",
                    ClosingTime = "23:00",
                    TotalTables = 20,
                    TotalCapacity = 60,
                    AveragePricePerPerson = 55,
                    IsActive = true,
                    IsBreakfastAvailable = false,
                    IsLunchAvailable = false,
                    IsDinnerAvailable = true,
                    Images = new List<string> { "rest3_1.jpg" },
                    CreatedAt = DateTime.Now.AddMonths(-3)
                }
            };
        }

        public static List<Menu> GetMenus()
        {
            return new List<Menu>
            {
                new Menu
                {
                    Id = 1,
                    RestaurantId = 1,
                    Name = "Breakfast Menu",
                    Description = "Morning breakfast selection",
                    IsActive = true,
                    ValidFrom = DateTime.Now.AddMonths(-6),
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new Menu
                {
                    Id = 2,
                    RestaurantId = 1,
                    Name = "Lunch Menu",
                    Description = "Lunch specials",
                    IsActive = true,
                    ValidFrom = DateTime.Now.AddMonths(-6),
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new Menu
                {
                    Id = 3,
                    RestaurantId = 1,
                    Name = "Dinner Menu",
                    Description = "Evening dining",
                    IsActive = true,
                    ValidFrom = DateTime.Now.AddMonths(-6),
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new Menu
                {
                    Id = 4,
                    RestaurantId = 2,
                    Name = "Italian Menu",
                    Description = "Authentic Italian dishes",
                    IsActive = true,
                    ValidFrom = DateTime.Now.AddMonths(-4),
                    CreatedAt = DateTime.Now.AddMonths(-4)
                }
            };
        }

        public static List<MenuItem> GetMenuItems()
        {
            var items = new List<MenuItem>();
            var categories = new[] { "Appetizer", "Main", "Dessert", "Beverage" };
            var names = new[] { "Margherita Pizza", "Caesar Salad", "Grilled Salmon", "Beef Steak", "Tiramisu", "Cheesecake", "Cappuccino", "Wine" };
            
            for (int i = 1; i <= 80; i++)
            {
                items.Add(new MenuItem
                {
                    Id = i,
                    MenuId = _random.Next(1, 5),
                    Name = names[_random.Next(names.Length)] + " " + i,
                    Description = "Delicious " + names[_random.Next(names.Length)].ToLower(),
                    Category = categories[_random.Next(categories.Length)],
                    Price = _random.Next(10, 80),
                    Currency = "EUR",
                    IsVegetarian = _random.Next(0, 10) > 7,
                    IsVegan = _random.Next(0, 10) > 8,
                    IsGlutenFree = _random.Next(0, 10) > 8,
                    PreparationTimeMinutes = _random.Next(5, 30),
                    Calories = _random.Next(200, 1200),
                    Ingredients = new List<string> { "Ingredient 1", "Ingredient 2" },
                    Allergens = new List<string>(),
                    IsAvailable = _random.Next(0, 10) > 1,
                    StockQuantity = _random.Next(0, 100),
                    CreatedAt = DateTime.Now.AddMonths(-_random.Next(1, 6))
                });
            }
            
            return items;
        }

        public static List<Table> GetTables()
        {
            var tables = new List<Table>();
            var restaurants = GetRestaurants();
            var locations = new[] { "Indoor", "Outdoor", "Terrace", "Private" };
            var statuses = new[] { "Available", "Occupied", "Reserved", "Cleaning" };
            
            foreach (var restaurant in restaurants)
            {
                for (int i = 1; i <= restaurant.TotalTables; i++)
                {
                    tables.Add(new Table
                    {
                        Id = tables.Count + 1,
                        RestaurantId = restaurant.Id,
                        TableNumber = i.ToString(),
                        Capacity = i <= 10 ? 2 : (i <= 30 ? 4 : (i <= 45 ? 6 : 8)),
                        Location = locations[_random.Next(locations.Length)],
                        IsSmoking = i % 5 == 0,
                        HasView = i % 3 == 0,
                        IsActive = true,
                        Status = statuses[_random.Next(statuses.Length)],
                        CreatedAt = DateTime.Now.AddMonths(-6)
                    });
                }
            }
            
            return tables;
        }

        public static List<Order> GetOrders()
        {
            var orders = new List<Order>();
            var statuses = new[] { "Pending", "Preparing", "Ready", "Served", "Completed", "Cancelled" };
            var orderTypes = new[] { "DineIn", "Takeaway", "RoomService" };
            
            for (int i = 1; i <= 200; i++)
            {
                var status = statuses[_random.Next(statuses.Length)];
                var orderTime = DateTime.Now.AddHours(-_random.Next(0, 48));
                var total = _random.Next(50, 500);
                
                orders.Add(new Order
                {
                    Id = i,
                    OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{i:D4}",
                    RestaurantId = _random.Next(1, 4),
                    TableId = _random.Next(1, 100),
                    TableNumber = _random.Next(1, 50).ToString(),
                    CustomerName = $"Customer {_random.Next(1, 100)}",
                    OrderType = orderTypes[_random.Next(orderTypes.Length)],
                    Status = status,
                    SubTotal = total,
                    TaxAmount = total * 0.1m,
                    TaxRate = 10,
                    TotalAmount = total * 1.1m,
                    Currency = "EUR",
                    OrderTime = orderTime,
                    CreatedAt = orderTime
                });
            }
            
            return orders;
        }

        public static List<OrderItem> GetOrderItems()
        {
            var items = new List<OrderItem>();
            var menuItems = GetMenuItems();
            var orders = GetOrders();
            
            foreach (var order in orders)
            {
                var itemCount = _random.Next(1, 5);
                for (int i = 0; i < itemCount; i++)
                {
                    var menuItem = menuItems[_random.Next(menuItems.Count)];
                    var quantity = _random.Next(1, 3);
                    items.Add(new OrderItem
                    {
                        Id = items.Count + 1,
                        OrderId = order.Id,
                        MenuItemId = menuItem.Id,
                        ItemName = menuItem.Name,
                        Quantity = quantity,
                        UnitPrice = menuItem.Price,
                        TotalPrice = menuItem.Price * quantity,
                        Status = order.Status == "Completed" ? "Served" : order.Status
                    });
                }
            }
            
            return items;
        }

        public static List<TableReservation> GetReservations()
        {
            var reservations = new List<TableReservation>();
            var statuses = new[] { "Pending", "Confirmed", "Arrived", "Cancelled", "NoShow" };
            
            for (int i = 1; i <= 100; i++)
            {
                var reservationDate = DateTime.Now.AddDays(_random.Next(-30, 30));
                reservations.Add(new TableReservation
                {
                    Id = i,
                    ReservationNumber = $"RES-{DateTime.Now:yyyyMMdd}-{i:D4}",
                    RestaurantId = _random.Next(1, 4),
                    TableId = _random.Next(1, 100),
                    TableNumber = _random.Next(1, 50).ToString(),
                    CustomerName = $"Customer {_random.Next(1, 100)}",
                    CustomerEmail = $"customer{_random.Next(1, 100)}@email.com",
                    CustomerPhone = $"+90 5{_random.Next(10, 99)} {_random.Next(100, 999)} {_random.Next(1000, 9999)}",
                    GuestCount = _random.Next(2, 8),
                    ReservationDate = reservationDate,
                    ReservationTime = $"{_random.Next(12, 22)}:00",
                    DurationMinutes = 120,
                    Status = statuses[_random.Next(statuses.Length)],
                    CreatedAt = reservationDate.AddDays(-_random.Next(1, 30))
                });
            }
            
            return reservations;
        }

        public static List<StockItem> GetStockItems()
        {
            var items = new List<StockItem>();
            var categories = new[] { "Vegetables", "Meat", "Dairy", "Beverages", "Dry Goods" };
            var units = new[] { "kg", "lt", "piece", "box" };
            
            for (int i = 1; i <= 50; i++)
            {
                items.Add(new StockItem
                {
                    Id = i,
                    RestaurantId = _random.Next(1, 4),
                    Name = $"Stock Item {i}",
                    Category = categories[_random.Next(categories.Length)],
                    Unit = units[_random.Next(units.Length)],
                    CurrentStock = _random.Next(0, 500),
                    MinStockLevel = _random.Next(10, 50),
                    ReorderLevel = _random.Next(20, 100),
                    ReorderQuantity = _random.Next(50, 200),
                    UnitPrice = _random.Next(5, 100),
                    Supplier = $"Supplier {_random.Next(1, 10)}",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-_random.Next(1, 12))
                });
            }
            
            return items;
        }
    }
}