using MICE.API.Models;

namespace MICE.API.Repositories
{
    public interface IAttendeeRepository
    {
        Task<EventAttendee> GetByIdAsync(int id);
        Task<List<EventAttendee>> GetByEventAsync(int eventId);
        Task<List<EventAttendee>> GetCheckedInAttendeesAsync(int eventId);
        Task<EventAttendee> AddAsync(EventAttendee attendee);
        Task<EventAttendee> UpdateAsync(EventAttendee attendee);
        Task<EventAttendee> CheckInAsync(int attendeeId);
        Task<bool> DeleteAsync(int id);
        Task<int> GetAttendeeCountAsync(int eventId);
        Task<EventAttendee> GetByQrCodeAsync(string qrCode);
    }
}