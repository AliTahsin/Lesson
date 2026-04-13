using RoomManagement.API.Models;
using System.Collections.Generic;

namespace RoomManagement.API.Data
{
    public static class MockData
    {
        public static List<RoomType> GetRoomTypes()
        {
            return new List<RoomType>
            {
                new RoomType 
                { 
                    Id = 1, Name = "Standard", Code = "STD", 
                    Description = "Comfortable standard room with essential amenities",
                    DefaultCapacity = 2, DefaultPrice = 150, Icon = "bed",
                    StandardAmenities = new List<string> { "WiFi", "TV", "Mini Bar", "Air Conditioning" },
                    ImageUrl = "standard_room.jpg"
                },
                new RoomType 
                { 
                    Id = 2, Name = "Deluxe", Code = "DLX", 
                    Description = "Spacious deluxe room with premium amenities",
                    DefaultCapacity = 3, DefaultPrice = 250, Icon = "star",
                    StandardAmenities = new List<string> { "WiFi", "TV", "Mini Bar", "Jacuzzi", "Sea View" },
                    ImageUrl = "deluxe_room.jpg"
                },
                new RoomType 
                { 
                    Id = 3, Name = "Suite", Code = "STE", 
                    Description = "Luxury suite with separate living area",
                    DefaultCapacity = 4, DefaultPrice = 450, Icon = "crown",
                    StandardAmenities = new List<string> { "WiFi", "TV", "Mini Bar", "Jacuzzi", "Living Room", "Kitchenette" },
                    ImageUrl = "suite_room.jpg"
                },
                new RoomType 
                { 
                    Id = 4, Name = "Presidential", Code = "PRS", 
                    Description = "Ultimate luxury presidential suite",
                    DefaultCapacity = 6, DefaultPrice = 1200, Icon = "diamond",
                    StandardAmenities = new List<string> { "WiFi", "TV", "Mini Bar", "Jacuzzi", "Private Pool", "Butler Service", "Private Terrace" },
                    ImageUrl = "presidential_room.jpg"
                },
                new RoomType 
                { 
                    Id = 5, Name = "Family", Code = "FAM", 
                    Description = "Perfect for families with children",
                    DefaultCapacity = 4, DefaultPrice = 200, Icon = "users",
                    StandardAmenities = new List<string> { "WiFi", "TV", "Mini Bar", "Kids Corner", "PlayStation" },
                    ImageUrl = "family_room.jpg"
                }
            };
        }

        public static List<Room> GetRooms()
        {
            var rooms = new List<Room>();
            var hotels = new[] { 
                new { Id = 1, Name = "Marriott Istanbul" },
                new { Id = 2, Name = "Hilton Izmir" },
                new { Id = 3, Name = "Sofitel Bodrum" }
            };

            var roomTypes = GetRoomTypes();
            var roomCounter = 1;

            foreach (var hotel in hotels)
            {
                foreach (var roomType in roomTypes)
                {
                    for (int i = 1; i <= 10; i++) // Her otelde her tipten 10 oda
                    {
                        rooms.Add(new Room
                        {
                            Id = roomCounter++,
                            HotelId = hotel.Id,
                            HotelName = hotel.Name,
                            RoomTypeId = roomType.Id,
                            RoomNumber = $"{roomType.Code}{i:D3}",
                            Floor = (i % 5) + 1,
                            View = i % 4 == 0 ? "Sea" : (i % 3 == 0 ? "City" : "Garden"),
                            Capacity = roomType.DefaultCapacity,
                            ExtraBedCapacity = roomType.DefaultCapacity >= 4 ? 1 : 0,
                            BasePrice = roomType.DefaultPrice,
                            IsAvailable = i % 10 != 0, // %90 müsait
                            IsClean = i % 15 != 0,
                            Status = i % 10 == 0 ? "Maintenance" : (i % 7 == 0 ? "Cleaning" : "Available"),
                            Amenities = roomType.StandardAmenities,
                            Description = $"{roomType.Name} room at {hotel.Name} with {roomType.Description}"
                        });
                    }
                }
            }

            return rooms;
        }

        public static List<RoomInventory> GetRoomInventories(DateTime startDate, DateTime endDate)
        {
            var inventories = new List<RoomInventory>();
            var rooms = GetRooms();
            var random = new Random();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                foreach (var room in rooms)
                {
                    var bookedCount = random.Next(0, 3);
                    var maintenanceCount = random.Next(0, 1);
                    
                    inventories.Add(new RoomInventory
                    {
                        Id = inventories.Count + 1,
                        RoomId = room.Id,
                        Date = date,
                        IsAvailable = room.IsAvailable && bookedCount == 0,
                        Price = room.BasePrice * (1 + (random.Next(-20, 30) / 100m)),
                        AvailableCount = 1,
                        BookedCount = bookedCount,
                        MaintenanceCount = maintenanceCount
                    });
                }
            }

            return inventories;
        }
    }
}