using MICE.API.DTOs;

namespace MICE.API.Services
{
    public interface IEventService
    {
        Task<EventDto> GetEventByIdAsync(int id);
        Task<EventDto> GetEventByNumberAsync(string eventNumber);
        Task<List<EventDto>> GetEventsByHotelAsync(int hotelId);
        Task<List<EventDto>> GetUpcomingEventsAsync(int hotelId, int days = 7);
        Task<EventDto> CreateEventAsync(CreateEventDto dto);
        Task<EventDto> UpdateEventAsync(int id, UpdateEventDto dto);
        Task<EventDto> UpdateEventStatusAsync(int id, string status);
        Task<bool> DeleteEventAsync(int id);
        Task<byte[]> GenerateCalendarFileAsync(int eventId);
    }
}