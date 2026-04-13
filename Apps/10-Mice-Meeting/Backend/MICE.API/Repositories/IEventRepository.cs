using MICE.API.Models;

namespace MICE.API.Repositories
{
    public interface IEventRepository
    {
        Task<Event> GetByIdAsync(int id);
        Task<Event> GetByEventNumberAsync(string eventNumber);
        Task<List<Event>> GetAllAsync();
        Task<List<Event>> GetByHotelAsync(int hotelId);
        Task<List<Event>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Event>> GetByStatusAsync(string status);
        Task<Event> AddAsync(Event eventItem);
        Task<Event> UpdateAsync(Event eventItem);
        Task<bool> DeleteAsync(int id);
        Task<Event> UpdateStatusAsync(int id, string status);
        Task<List<Event>> GetUpcomingEventsAsync(int hotelId, int days = 7);
    }
}