using MICE.API.DTOs;

namespace MICE.API.Services
{
    public interface IMeetingRoomService
    {
        Task<MeetingRoomDto> GetRoomByIdAsync(int id);
        Task<List<MeetingRoomDto>> GetRoomsByHotelAsync(int hotelId);
        Task<List<MeetingRoomDto>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate, int capacity);
        Task<MeetingRoomDto> CreateRoomAsync(CreateMeetingRoomDto dto);
        Task<MeetingRoomDto> UpdateRoomAsync(int id, UpdateMeetingRoomDto dto);
        Task<bool> DeleteRoomAsync(int id);
        Task<bool> CheckAvailabilityAsync(int roomId, DateTime startDate, DateTime endDate);
    }
}