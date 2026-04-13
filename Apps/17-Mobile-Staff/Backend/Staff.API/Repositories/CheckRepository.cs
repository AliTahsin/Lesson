using Staff.API.Models;
using Staff.API.Data;

namespace Staff.API.Repositories
{
    public class CheckRepository : ICheckRepository
    {
        private readonly List<CheckInOut> _checks;

        public CheckRepository()
        {
            _checks = MockData.GetCheckInOuts();
        }

        public async Task<CheckInOut> GetByIdAsync(int id)
        {
            return await Task.FromResult(_checks.FirstOrDefault(c => c.Id == id));
        }

        public async Task<List<CheckInOut>> GetByReservationAsync(int reservationId)
        {
            return await Task.FromResult(_checks.Where(c => c.ReservationId == reservationId).ToList());
        }

        public async Task<List<CheckInOut>> GetByDateAsync(DateTime date)
        {
            return await Task.FromResult(_checks.Where(c => c.ProcessedAt.Date == date.Date).ToList());
        }

        public async Task<CheckInOut> AddAsync(CheckInOut check)
        {
            check.Id = _checks.Max(c => c.Id) + 1;
            check.ProcessedAt = DateTime.Now;
            _checks.Add(check);
            return await Task.FromResult(check);
        }

        public async Task<bool> HasCheckedInAsync(int reservationId)
        {
            return await Task.FromResult(_checks.Any(c => c.ReservationId == reservationId && c.Type == "CheckIn"));
        }

        public async Task<bool> HasCheckedOutAsync(int reservationId)
        {
            return await Task.FromResult(_checks.Any(c => c.ReservationId == reservationId && c.Type == "CheckOut"));
        }
    }
}