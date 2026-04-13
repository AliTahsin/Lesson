using RoomManagement.API.Models;
using RoomManagement.API.DTOs;
using RoomManagement.API.Data;

namespace RoomManagement.API.Services
{
    public class RoomManagementService
    {
        private readonly List<Room> _rooms;
        private readonly List<RoomType> _roomTypes;
        private List<RoomInventory> _inventories;

        public RoomManagementService()
        {
            _rooms = MockData.GetRooms();
            _roomTypes = MockData.GetRoomTypes();
            _inventories = MockData.GetRoomInventories(DateTime.Today, DateTime.Today.AddDays(90));
        }

        // Room Operations
        public List<Room> GetAllRooms() => _rooms;
        
        public Room GetRoomById(int id) => _rooms.FirstOrDefault(r => r.Id == id);
        
        public List<Room> GetRoomsByHotel(int hotelId) => _rooms.Where(r => r.HotelId == hotelId).ToList();
        
        public List<Room> GetRoomsByType(int roomTypeId) => _rooms.Where(r => r.RoomTypeId == roomTypeId).ToList();
        
        public List<Room> GetAvailableRooms(int hotelId, DateTime checkIn, DateTime checkOut)
        {
            var bookedRoomIds = _inventories
                .Where(i => i.Date >= checkIn && i.Date < checkOut && !i.IsAvailable)
                .Select(i => i.RoomId)
                .Distinct()
                .ToList();

            return _rooms
                .Where(r => r.HotelId == hotelId && r.IsAvailable && !bookedRoomIds.Contains(r.Id))
                .ToList();
        }

        public List<Room> SearchRooms(int? hotelId, int? roomTypeId, int? capacity, string view, decimal? minPrice, decimal? maxPrice)
        {
            var query = _rooms.AsQueryable();

            if (hotelId.HasValue)
                query = query.Where(r => r.HotelId == hotelId.Value);
            if (roomTypeId.HasValue)
                query = query.Where(r => r.RoomTypeId == roomTypeId.Value);
            if (capacity.HasValue)
                query = query.Where(r => r.Capacity >= capacity.Value);
            if (!string.IsNullOrEmpty(view))
                query = query.Where(r => r.View == view);
            if (minPrice.HasValue)
                query = query.Where(r => r.BasePrice >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(r => r.BasePrice <= maxPrice.Value);

            return query.ToList();
        }

        public Room AddRoom(Room room)
        {
            room.Id = _rooms.Max(r => r.Id) + 1;
            _rooms.Add(room);
            return room;
        }

        public bool UpdateRoom(int id, Room updatedRoom)
        {
            var room = GetRoomById(id);
            if (room == null) return false;

            room.RoomNumber = updatedRoom.RoomNumber;
            room.Floor = updatedRoom.Floor;
            room.View = updatedRoom.View;
            room.Capacity = updatedRoom.Capacity;
            room.BasePrice = updatedRoom.BasePrice;
            room.IsAvailable = updatedRoom.IsAvailable;
            room.Status = updatedRoom.Status;
            room.Amenities = updatedRoom.Amenities;

            return true;
        }

        public bool UpdateRoomStatus(int id, string status)
        {
            var room = GetRoomById(id);
            if (room == null) return false;

            room.Status = status;
            room.IsAvailable = status == "Available";
            return true;
        }

        // RoomType Operations
        public List<RoomType> GetAllRoomTypes() => _roomTypes;
        public RoomType GetRoomTypeById(int id) => _roomTypes.FirstOrDefault(rt => rt.Id == id);

        // Inventory Operations
        public List<RoomInventory> GetInventoryByRoom(int roomId, DateTime startDate, DateTime endDate)
        {
            return _inventories
                .Where(i => i.RoomId == roomId && i.Date >= startDate && i.Date <= endDate)
                .OrderBy(i => i.Date)
                .ToList();
        }

        public RoomInventory UpdateInventoryPrice(int roomId, DateTime date, decimal price)
        {
            var inventory = _inventories.FirstOrDefault(i => i.RoomId == roomId && i.Date == date);
            if (inventory == null) return null;

            inventory.Price = price;
            return inventory;
        }

        public object GetRoomStatistics(int hotelId)
        {
            var hotelRooms = _rooms.Where(r => r.HotelId == hotelId).ToList();
            var today = DateTime.Today;
            var todayInventory = _inventories.Where(i => i.Date == today).ToList();

            return new
            {
                TotalRooms = hotelRooms.Count,
                AvailableRooms = hotelRooms.Count(r => r.Status == "Available"),
                OccupiedRooms = hotelRooms.Count(r => r.Status == "Occupied"),
                MaintenanceRooms = hotelRooms.Count(r => r.Status == "Maintenance"),
                CleaningRooms = hotelRooms.Count(r => r.Status == "Cleaning"),
                ByRoomType = hotelRooms.GroupBy(r => r.RoomTypeId).Select(g => new
                {
                    RoomType = _roomTypes.FirstOrDefault(rt => rt.Id == g.Key)?.Name,
                    Count = g.Count()
                }),
                AveragePrice = hotelRooms.Average(r => r.BasePrice),
                TodayRevenue = todayInventory.Sum(i => i.BookedCount * i.Price)
            };
        }
    }
}