using MICE.API.Models;
using MICE.API.Data;

namespace MICE.API.Repositories
{
    public class AttendeeRepository : IAttendeeRepository
    {
        private readonly List<EventAttendee> _attendees;

        public AttendeeRepository()
        {
            _attendees = MockData.GetAttendees();
        }

        public async Task<EventAttendee> GetByIdAsync(int id)
        {
            return await Task.FromResult(_attendees.FirstOrDefault(a => a.Id == id));
        }

        public async Task<List<EventAttendee>> GetByEventAsync(int eventId)
        {
            return await Task.FromResult(_attendees.Where(a => a.EventId == eventId).ToList());
        }

        public async Task<List<EventAttendee>> GetCheckedInAttendeesAsync(int eventId)
        {
            return await Task.FromResult(_attendees.Where(a => a.EventId == eventId && a.HasCheckedIn).ToList());
        }

        public async Task<EventAttendee> AddAsync(EventAttendee attendee)
        {
            attendee.Id = _attendees.Max(a => a.Id) + 1;
            attendee.RegisteredAt = DateTime.Now;
            attendee.Status = "Registered";
            attendee.QrCode = GenerateQrCode(attendee.Id);
            _attendees.Add(attendee);
            return await Task.FromResult(attendee);
        }

        public async Task<EventAttendee> UpdateAsync(EventAttendee attendee)
        {
            var existing = await GetByIdAsync(attendee.Id);
            if (existing != null)
            {
                var index = _attendees.IndexOf(existing);
                _attendees[index] = attendee;
            }
            return await Task.FromResult(attendee);
        }

        public async Task<EventAttendee> CheckInAsync(int attendeeId)
        {
            var attendee = await GetByIdAsync(attendeeId);
            if (attendee != null)
            {
                attendee.HasCheckedIn = true;
                attendee.CheckInTime = DateTime.Now;
                attendee.Status = "CheckedIn";
                await UpdateAsync(attendee);
            }
            return await Task.FromResult(attendee);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var attendee = await GetByIdAsync(id);
            if (attendee != null)
            {
                _attendees.Remove(attendee);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<int> GetAttendeeCountAsync(int eventId)
        {
            return await Task.FromResult(_attendees.Count(a => a.EventId == eventId));
        }

        public async Task<EventAttendee> GetByQrCodeAsync(string qrCode)
        {
            return await Task.FromResult(_attendees.FirstOrDefault(a => a.QrCode == qrCode));
        }

        private string GenerateQrCode(int id)
        {
            return $"QR-{DateTime.Now.Ticks}-{id}";
        }
    }
}