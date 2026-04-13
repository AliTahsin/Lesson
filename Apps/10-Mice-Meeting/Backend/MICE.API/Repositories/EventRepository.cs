using MICE.API.Models;
using MICE.API.Data;

namespace MICE.API.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly List<Event> _events;

        public EventRepository()
        {
            _events = MockData.GetEvents();
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            var eventItem = await Task.FromResult(_events.FirstOrDefault(e => e.Id == id));
            if (eventItem != null)
            {
                eventItem.Schedule = MockData.GetEventSchedules().Where(s => s.EventId == id).ToList();
            }
            return eventItem;
        }

        public async Task<Event> GetByEventNumberAsync(string eventNumber)
        {
            return await Task.FromResult(_events.FirstOrDefault(e => e.EventNumber == eventNumber));
        }

        public async Task<List<Event>> GetAllAsync()
        {
            return await Task.FromResult(_events.ToList());
        }

        public async Task<List<Event>> GetByHotelAsync(int hotelId)
        {
            return await Task.FromResult(_events.Where(e => e.HotelId == hotelId).ToList());
        }

        public async Task<List<Event>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(_events.Where(e => 
                e.StartDate >= startDate && e.EndDate <= endDate).ToList());
        }

        public async Task<List<Event>> GetByStatusAsync(string status)
        {
            return await Task.FromResult(_events.Where(e => e.Status == status).ToList());
        }

        public async Task<Event> AddAsync(Event eventItem)
        {
            eventItem.Id = _events.Max(e => e.Id) + 1;
            eventItem.EventNumber = $"EVT-{DateTime.Now.Year}-{eventItem.Id:D4}";
            eventItem.CreatedAt = DateTime.Now;
            _events.Add(eventItem);
            return await Task.FromResult(eventItem);
        }

        public async Task<Event> UpdateAsync(Event eventItem)
        {
            var existing = await GetByIdAsync(eventItem.Id);
            if (existing != null)
            {
                var index = _events.IndexOf(existing);
                eventItem.UpdatedAt = DateTime.Now;
                _events[index] = eventItem;
            }
            return await Task.FromResult(eventItem);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var eventItem = await GetByIdAsync(id);
            if (eventItem != null)
            {
                _events.Remove(eventItem);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<Event> UpdateStatusAsync(int id, string status)
        {
            var eventItem = await GetByIdAsync(id);
            if (eventItem != null)
            {
                eventItem.Status = status;
                switch (status)
                {
                    case "Confirmed":
                        eventItem.ConfirmedAt = DateTime.Now;
                        break;
                    case "Completed":
                        eventItem.CompletedAt = DateTime.Now;
                        break;
                }
                eventItem.UpdatedAt = DateTime.Now;
            }
            return await Task.FromResult(eventItem);
        }

        public async Task<List<Event>> GetUpcomingEventsAsync(int hotelId, int days = 7)
        {
            var endDate = DateTime.Now.AddDays(days);
            return await Task.FromResult(_events
                .Where(e => e.HotelId == hotelId && 
                           e.StartDate >= DateTime.Now && 
                           e.StartDate <= endDate &&
                           e.Status != "Cancelled")
                .OrderBy(e => e.StartDate)
                .ToList());
        }
    }
}