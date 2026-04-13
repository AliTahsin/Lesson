using Staff.API.Models;

namespace Staff.API.Repositories
{
    public interface ICheckRepository
    {
        Task<CheckInOut> GetByIdAsync(int id);
        Task<List<CheckInOut>> GetByReservationAsync(int reservationId);
        Task<List<CheckInOut>> GetByDateAsync(DateTime date);
        Task<CheckInOut> AddAsync(CheckInOut check);
        Task<bool> HasCheckedInAsync(int reservationId);
        Task<bool> HasCheckedOutAsync(int reservationId);
    }
}