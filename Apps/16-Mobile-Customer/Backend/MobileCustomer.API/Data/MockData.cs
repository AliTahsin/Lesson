using MobileCustomer.API.Models;

namespace MobileCustomer.API.Data
{
    public static class MockData
    {
        private static readonly Random _random = new();

        public static List<CustomerProfile> GetProfiles()
        {
            var profiles = new List<CustomerProfile>();
            
            for (int i = 1; i <= 20; i++)
            {
                profiles.Add(new CustomerProfile
                {
                    Id = i,
                    UserId = i,
                    FirstName = GetRandomFirstName(),
                    LastName = GetRandomLastName(),
                    Email = $"user{i}@email.com",
                    PhoneNumber = $"+90555{_random.Next(1000000, 9999999)}",
                    DateOfBirth = GetRandomDateOfBirth(),
                    Country = GetRandomCountry(),
                    City = GetRandomCity(),
                    Address = $"{_random.Next(1, 999)} {GetRandomStreet()}, {GetRandomCity()}",
                    Language = GetRandomLanguage(),
                    BiometricEnabled = _random.Next(0, 10) > 6,
                    BiometricKey = _random.Next(0, 10) > 6 ? Guid.NewGuid().ToString() : null,
                    PushEnabled = true,
                    EmailEnabled = true,
                    SmsEnabled = _random.Next(0, 10) > 7,
                    ProfileImageUrl = _random.Next(0, 10) > 8 ? $"https://randomuser.me/api/portraits/{_random.Next(0, 10) > 5 ? "men" : "women"}/{_random.Next(1, 100)}.jpg" : null,
                    CreatedAt = DateTime.Now.AddMonths(-_random.Next(1, 12)),
                    LastLoginAt = DateTime.Now.AddDays(-_random.Next(0, 30)),
                    UpdatedAt = _random.Next(0, 10) > 7 ? DateTime.Now.AddDays(-_random.Next(1, 15)) : null
                });
            }
            
            return profiles;
        }

        public static List<DigitalKey> GetDigitalKeys()
        {
            var keys = new List<DigitalKey>();
            
            for (int i = 1; i <= 30; i++)
            {
                var validUntil = DateTime.Now.AddDays(_random.Next(1, 7));
                var isActive = validUntil > DateTime.Now;
                var isUsed = _random.Next(0, 10) > 8;
                
                keys.Add(new DigitalKey
                {
                    Id = i,
                    ReservationId = 100 + i,
                    UserId = _random.Next(1, 10),
                    RoomId = _random.Next(1, 50),
                    RoomNumber = $"{_random.Next(1, 5)}{_random.Next(100, 500)}",
                    KeyCode = GenerateKeyCode(),
                    QrCode = GenerateQrCode(i),
                    ValidFrom = DateTime.Now.AddDays(-1),
                    ValidUntil = validUntil,
                    IsActive = isActive && !isUsed,
                    IsUsed = isUsed,
                    UsedAt = isUsed ? DateTime.Now.AddHours(-_random.Next(1, 24)) : null,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    UpdatedAt = null
                });
            }
            
            return keys;
        }

        public static List<RoomServiceOrder> GetRoomServiceOrders()
        {
            var orders = new List<RoomServiceOrder>();
            var statuses = new[] { "Pending", "Preparing", "OnTheWay", "Delivered", "Cancelled" };
            var menuItems = GetMenuItems();
            
            for (int i = 1; i <= 50; i++)
            {
                var status = statuses[_random.Next(statuses.Length)];
                var orderTime = DateTime.Now.AddMinutes(-_random.Next(30, 180));
                var itemCount = _random.Next(1, 4);
                var items = new List<OrderItem>();
                var subTotal = 0m;
                
                for (int j = 0; j < itemCount; j++)
                {
                    var menuItem = menuItems[_random.Next(menuItems.Count)];
                    var quantity = _random.Next(1, 3);
                    var totalPrice = menuItem.Price * quantity;
                    subTotal += totalPrice;
                    
                    items.Add(new OrderItem
                    {
                        Id = j + 1,
                        OrderId = i,
                        ItemName = menuItem.Name,
                        Quantity = quantity,
                        UnitPrice = menuItem.Price,
                        TotalPrice = totalPrice,
                        SpecialInstructions = _random.Next(0, 10) > 8 ? "Extra spicy please" : null
                    });
                }
                
                var taxAmount = subTotal * 0.10m;
                var totalAmount = subTotal + taxAmount;
                
                orders.Add(new RoomServiceOrder
                {
                    Id = i,
                    OrderNumber = $"RSO-{DateTime.Now.Year}{i:D4}",
                    UserId = _random.Next(1, 10),
                    ReservationId = 100 + _random.Next(1, 20),
                    RoomId = _random.Next(1, 50),
                    RoomNumber = $"{_random.Next(1, 5)}{_random.Next(100, 500)}",
                    Items = items,
                    SubTotal = subTotal,
                    TaxAmount = taxAmount,
                    TotalAmount = totalAmount,
                    Currency = "EUR",
                    SpecialInstructions = _random.Next(0, 10) > 8 ? "Please call before coming" : null,
                    Status = status,
                    OrderTime = orderTime,
                    EstimatedDeliveryTime = status != "Delivered" ? orderTime.AddMinutes(_random.Next(20, 60)) : null,
                    DeliveredAt = status == "Delivered" ? orderTime.AddMinutes(_random.Next(20, 60)) : null,
                    CreatedAt = orderTime,
                    UpdatedAt = status != "Pending" ? orderTime.AddMinutes(_random.Next(5, 30)) : null
                });
            }
            
            return orders.OrderByDescending(o => o.OrderTime).ToList();
        }

        public static List<SpaAppointment> GetSpaAppointments()
        {
            var appointments = new List<SpaAppointment>();
            var services = GetSpaServices();
            var statuses = new[] { "Pending", "Confirmed", "Completed", "Cancelled" };
            
            for (int i = 1; i <= 40; i++)
            {
                var service = services[_random.Next(services.Count)];
                var status = statuses[_random.Next(statuses.Length)];
                var appointmentDate = DateTime.Now.AddDays(_random.Next(-5, 10));
                
                appointments.Add(new SpaAppointment
                {
                    Id = i,
                    AppointmentNumber = $"SPA-{DateTime.Now.Year}{i:D4}",
                    UserId = _random.Next(1, 10),
                    ReservationId = 100 + _random.Next(1, 20),
                    ServiceName = service.Name,
                    ServiceType = service.Category,
                    DurationMinutes = service.DurationMinutes,
                    Price = service.Price,
                    AppointmentDate = appointmentDate,
                    AppointmentTime = $"{_random.Next(9, 20)}:{(_random.Next(0, 2) * 30):D2}",
                    SpecialRequests = _random.Next(0, 10) > 8 ? "Prefer female therapist" : null,
                    Status = status,
                    CreatedAt = appointmentDate.AddDays(-_random.Next(1, 10)),
                    ConfirmedAt = status == "Confirmed" ? appointmentDate.AddDays(-_random.Next(1, 5)) : null,
                    CompletedAt = status == "Completed" ? appointmentDate.AddHours(_random.Next(1, 3)) : null
                });
            }
            
            return appointments.OrderByDescending(a => a.CreatedAt).ToList();
        }

        public static List<DeviceInfo> GetDeviceInfos()
        {
            var devices = new List<DeviceInfo>();
            var platforms = new[] { "iOS", "Android" };
            var deviceNames = new[] { "iPhone 14 Pro", "iPhone 15", "Samsung Galaxy S23", "Google Pixel 8", "Xiaomi 13" };
            
            for (int i = 1; i <= 25; i++)
            {
                devices.Add(new DeviceInfo
                {
                    Id = i,
                    UserId = _random.Next(1, 10),
                    DeviceId = Guid.NewGuid().ToString(),
                    DeviceName = deviceNames[_random.Next(deviceNames.Length)],
                    Platform = platforms[_random.Next(platforms.Length)],
                    OsVersion = _random.Next(0, 10) > 5 ? "17.1" : "14.0",
                    AppVersion = "1.0." + _random.Next(0, 10),
                    PushToken = Guid.NewGuid().ToString(),
                    IsActive = _random.Next(0, 10) > 2,
                    LastActiveAt = DateTime.Now.AddMinutes(-_random.Next(0, 1440)),
                    CreatedAt = DateTime.Now.AddDays(-_random.Next(1, 60)),
                    UpdatedAt = _random.Next(0, 10) > 7 ? DateTime.Now.AddDays(-_random.Next(1, 30)) : null
                });
            }
            
            return devices;
        }

        // Helper methods
        private static string GetRandomFirstName()
        {
            var names = new[] { "Ahmet", "Mehmet", "Ali", "Ayşe", "Fatma", "Zeynep", "John", "Jane", "Michael", "Sarah" };
            return names[_random.Next(names.Length)];
        }

        private static string GetRandomLastName()
        {
            var names = new[] { "Yılmaz", "Demir", "Kaya", "Çelik", "Şahin", "Smith", "Johnson", "Williams", "Brown", "Jones" };
            return names[_random.Next(names.Length)];
        }

        private static DateTime GetRandomDateOfBirth()
        {
            var start = new DateTime(1960, 1, 1);
            var range = (DateTime.Today - start).Days;
            return start.AddDays(_random.Next(range));
        }

        private static string GetRandomCountry()
        {
            var countries = new[] { "Turkey", "USA", "UK", "Germany", "France", "Italy", "Spain", "Russia" };
            return countries[_random.Next(countries.Length)];
        }

        private static string GetRandomCity()
        {
            var cities = new[] { "Istanbul", "Ankara", "Izmir", "New York", "London", "Berlin", "Paris", "Rome", "Madrid", "Moscow" };
            return cities[_random.Next(cities.Length)];
        }

        private static string GetRandomStreet()
        {
            var streets = new[] { "Main St", "Broadway", "Independence Ave", "Liberty St", "Park Avenue", "Oak Street", "Maple Drive", "Cedar Road" };
            return streets[_random.Next(streets.Length)];
        }

        private static string GetRandomLanguage()
        {
            var languages = new[] { "tr", "en", "de", "ru" };
            return languages[_random.Next(languages.Length)];
        }

        private static string GenerateKeyCode()
        {
            return $"DK-{DateTime.Now.Ticks}-{_random.Next(1000, 9999)}";
        }

        private static string GenerateQrCode(int id)
        {
            return $"https://api.hotel.com/digital-key/qr-{id}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        private static List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>
            {
                new MenuItem { Name = "Caesar Salad", Price = 12.50m, Category = "Appetizer" },
                new MenuItem { Name = "Greek Salad", Price = 11.00m, Category = "Appetizer" },
                new MenuItem { Name = "Bruschetta", Price = 8.50m, Category = "Appetizer" },
                new MenuItem { Name = "Grilled Salmon", Price = 28.00m, Category = "Main Course" },
                new MenuItem { Name = "Beef Steak", Price = 35.00m, Category = "Main Course" },
                new MenuItem { Name = "Chicken Breast", Price = 22.00m, Category = "Main Course" },
                new MenuItem { Name = "Pasta Carbonara", Price = 18.00m, Category = "Main Course" },
                new MenuItem { Name = "Tiramisu", Price = 7.50m, Category = "Dessert" },
                new MenuItem { Name = "Cheesecake", Price = 8.00m, Category = "Dessert" },
                new MenuItem { Name = "Ice Cream", Price = 5.00m, Category = "Dessert" },
                new MenuItem { Name = "Cappuccino", Price = 4.50m, Category = "Beverage" },
                new MenuItem { Name = "Espresso", Price = 3.50m, Category = "Beverage" },
                new MenuItem { Name = "Orange Juice", Price = 5.00m, Category = "Beverage" },
                new MenuItem { Name = "Red Wine", Price = 12.00m, Category = "Beverage" },
                new MenuItem { Name = "White Wine", Price = 12.00m, Category = "Beverage" }
            };
        }

        private static List<SpaServiceItem> GetSpaServices()
        {
            return new List<SpaServiceItem>
            {
                new SpaServiceItem { Name = "Swedish Massage", Category = "Massage", DurationMinutes = 60, Price = 80 },
                new SpaServiceItem { Name = "Deep Tissue Massage", Category = "Massage", DurationMinutes = 60, Price = 90 },
                new SpaServiceItem { Name = "Hot Stone Massage", Category = "Massage", DurationMinutes = 75, Price = 110 },
                new SpaServiceItem { Name = "Facial Treatment", Category = "Facial", DurationMinutes = 45, Price = 70 },
                new SpaServiceItem { Name = "Anti-Aging Facial", Category = "Facial", DurationMinutes = 60, Price = 100 },
                new SpaServiceItem { Name = "Body Scrub", Category = "Body", DurationMinutes = 45, Price = 65 },
                new SpaServiceItem { Name = "Body Wrap", Category = "Body", DurationMinutes = 60, Price = 85 },
                new SpaServiceItem { Name = "Sauna Session", Category = "Wellness", DurationMinutes = 30, Price = 30 },
                new SpaServiceItem { Name = "Steam Room", Category = "Wellness", DurationMinutes = 30, Price = 30 },
                new SpaServiceItem { Name = "Manicure & Pedicure", Category = "Nail", DurationMinutes = 60, Price = 50 }
            };
        }

        private class MenuItem
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Category { get; set; }
        }

        private class SpaServiceItem
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public int DurationMinutes { get; set; }
            public decimal Price { get; set; }
        }
    }
} 