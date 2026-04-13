using MICE.API.Models;
using MICE.API.Data;

namespace MICE.API.Repositories
{
    public class MeetingRoomRepository : IMeetingRoomRepository
    {
        private readonly List<MeetingRoom> _rooms;
        private readonly List<Event> _events;

        public MeetingRoomRepository()
        {
            _rooms = MockData.GetMeetingRooms();
            _events = MockData.GetEvents();
        }

        public async Task<MeetingRoom> GetByIdAsync(int id)
        {
            return await Task.FromResult(_rooms.FirstOrDefault(r => r.Id == id));
        }

        public async Task<List<MeetingRoom>> GetAllAsync()
        {
            return await Task.FromResult(_rooms.ToList());
        }

        public async Task<List<MeetingRoom>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_rooms.Where(r => r.HotelId == hotelId).ToList());
        }

        public async Task<List<MeetingRoom>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate, int capacity)
        {
            var bookedRoomIds = _events
                .Where(e => e.Status != "Cancelled" && 
                           !(e.EndDate <= startDate || e.StartDate >= endDate))
                .Select(e => e.MeetingRoomId)
                .Distinct()
                .ToList();

            var availableRooms = _rooms
                .Where(r => r.IsActive && r.Capacity >= capacity && !bookedRoomIds.Contains(r.Id))
                .ToList();

            return await Task.FromResult(availableRooms);
        }

        public async Task<MeetingRoom> AddAsync(MeetingRoom room)
        {
            room.Id = _rooms.Max(r => r.Id) + 1;
            room.CreatedAt = DateTime.Now;
            _rooms.Add(room);
            return await Task.FromResult(room);
        }

        public async Task<MeetingRoom> UpdateAsync(MeetingRoom room)
        {
            var existing = await GetByIdAsync(room.Id);
            if (existing != null)
            {
                var index = _rooms.IndexOf(existing);
                room.UpdatedAt = DateTime.Now;
                _rooms[index] = room;
            }
            return await Task.FromResult(room);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await GetByIdAsync(id);
            if (room != null)
            {
                _rooms.Remove(room);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> CheckAvailabilityAsync(int roomId, DateTime startDate, DateTime endDate)
        {
            var overlappingEvent = _events.Any(e =>
                e.MeetingRoomId == roomId &&
                e.Status != "Cancelled" &&
                !(e.EndDate <= startDate || e.StartDate >= endDate));
            
            return await Task.FromResult(!overlappingEvent);
        }
    }
}