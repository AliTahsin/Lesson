using MICE.API.Models;

namespace MICE.API.Repositories
{
    public interface IMeetingRoomRepository
    {
        Task<MeetingRoom> GetByIdAsync(int id);
        Task<List<MeetingRoom>> GetAllAsync();
        Task<List<MeetingRoom>> GetByHotelAsync(int hotelId);
        Task<List<MeetingRoom>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate, int capacity);
        Task<MeetingRoom> AddAsync(MeetingRoom room);
        Task<MeetingRoom> UpdateAsync(MeetingRoom room);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckAvailabilityAsync(int roomId, DateTime startDate, DateTime endDate);
    }
}