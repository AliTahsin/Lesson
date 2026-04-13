using Restaurant.API.Models;
using Restaurant.API.Data;

namespace Restaurant.API.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly List<TableReservation> _reservations;
        private readonly List<Table> _tables;

        public ReservationRepository()
        {
            _reservations = MockData.GetReservations();
            _tables = MockData.GetTables();
        }

        public async Task<TableReservation> GetByIdAsync(int id)
        {
            return await Task.FromResult(_reservations.FirstOrDefault(r => r.Id == id));
        }

        public async Task<TableReservation> GetByNumberAsync(string reservationNumber)
        {
            return await Task.FromResult(_reservations.FirstOrDefault(r => r.ReservationNumber == reservationNumber));
        }

        public async Task<List<TableReservation>> GetByRestaurantAsync(int restaurantId)
        {
            return await Task.FromResult(_reservations.Where(r => r.RestaurantId == restaurantId).ToList());
        }

        public async Task<List<TableReservation>> GetByDateAsync(int restaurantId, DateTime date)
        {
            return await Task.FromResult(_reservations.Where(r => 
                r.RestaurantId == restaurantId && 
                r.ReservationDate.Date == date.Date).ToList());
        }

        public async Task<List<TableReservation>> GetByCustomerAsync(int customerId)
        {
            return await Task.FromResult(_reservations.Where(r => r.CustomerId == customerId).ToList());
        }

        public async Task<TableReservation> CreateAsync(TableReservation reservation)
        {
            reservation.Id = _reservations.Max(r => r.Id) + 1;
            reservation.ReservationNumber = $"RES-{DateTime.Now.Ticks}-{reservation.Id}";
            reservation.CreatedAt = DateTime.Now;
            reservation.Status = "Pending";
            _reservations.Add(reservation);
            return await Task.FromResult(reservation);
        }

        public async Task<TableReservation> UpdateAsync(TableReservation reservation)
        {
            var existing = await GetByIdAsync(reservation.Id);
            if (existing != null)
            {
                var index = _reservations.IndexOf(existing);
                _reservations[index] = reservation;
            }
            return await Task.FromResult(reservation);
        }

        public async Task<TableReservation> UpdateStatusAsync(int id, string status)
        {
            var reservation = await GetByIdAsync(id);
            if (reservation != null)
            {
                reservation.Status = status;
                switch (status)
                {
                    case "Confirmed":
                        reservation.ConfirmedAt = DateTime.Now;
                        break;
                    case "Arrived":
                        reservation.ArrivedAt = DateTime.Now;
                        break;
                    case "Cancelled":
                        reservation.CancelledAt = DateTime.Now;
                        break;
                }
            }
            return await Task.FromResult(reservation);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservation = await GetByIdAsync(id);
            if (reservation != null)
            {
                _reservations.Remove(reservation);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<Table>> GetAvailableTablesAsync(int restaurantId, DateTime date, string time, int guestCount)
        {
            var restaurantTables = _tables.Where(t => t.RestaurantId == restaurantId && t.IsActive).ToList();
            
            var reservedTableIds = _reservations
                .Where(r => r.RestaurantId == restaurantId && 
                           r.ReservationDate.Date == date.Date &&
                           r.ReservationTime == time &&
                           r.Status != "Cancelled")
                .Select(r => r.TableId)
                .ToList();
            
            var availableTables = restaurantTables
                .Where(t => !reservedTableIds.Contains(t.Id) && t.Capacity >= guestCount)
                .ToList();
            
            return await Task.FromResult(availableTables);
        }
    }
}