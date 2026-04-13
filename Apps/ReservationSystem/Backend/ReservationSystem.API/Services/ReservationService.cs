using ReservationSystem.API.Models;
using ReservationSystem.API.DTOs;
using ReservationSystem.API.Data;

namespace ReservationSystem.API.Services
{
    public class ReservationService
    {
        private readonly List<Reservation> _reservations;
        private readonly List<Guest> _guests;
        private readonly List<ReservationHistory> _histories;
        private readonly List<dynamic> _rooms;

        public ReservationService()
        {
            _reservations = MockData.GetReservations();
            _guests = MockData.GetGuests();
            _histories = MockData.GetReservationHistories();
            _rooms = GetRoomList();
        }

        private List<dynamic> GetRoomList()
        {
            return new List<dynamic>
            {
                new { Id = 1, HotelId = 1, HotelName = "Marriott Istanbul", RoomNumber = "101", RoomType = "Standard", Price = 150 },
                new { Id = 2, HotelId = 1, HotelName = "Marriott Istanbul", RoomNumber = "202", RoomType = "Deluxe", Price = 250 },
                new { Id = 3, HotelId = 1, HotelName = "Marriott Istanbul", RoomNumber = "305", RoomType = "Suite", Price = 450 },
                new { Id = 4, HotelId = 2, HotelName = "Hilton Izmir", RoomNumber = "110", RoomType = "Standard", Price = 120 },
                new { Id = 5, HotelId = 2, HotelName = "Hilton Izmir", RoomNumber = "208", RoomType = "Deluxe", Price = 200 },
                new { Id = 6, HotelId = 3, HotelName = "Sofitel Bodrum", RoomNumber = "405", RoomType = "Standard", Price = 180 },
                new { Id = 7, HotelId = 3, HotelName = "Sofitel Bodrum", RoomNumber = "501", RoomType = "Suite", Price = 500 }
            };
        }

        // Get all reservations
        public List<ReservationResponseDto> GetAllReservations()
        {
            return _reservations.Select(r => MapToResponseDto(r)).ToList();
        }

        // Get reservation by ID
        public ReservationResponseDto GetReservationById(int id)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == id);
            return reservation != null ? MapToResponseDto(reservation) : null;
        }

        // Get reservation by number
        public ReservationResponseDto GetReservationByNumber(string reservationNumber)
        {
            var reservation = _reservations.FirstOrDefault(r => r.ReservationNumber == reservationNumber);
            return reservation != null ? MapToResponseDto(reservation) : null;
        }

        // Get reservations by guest email
        public List<ReservationResponseDto> GetReservationsByGuestEmail(string email)
        {
            var guest = _guests.FirstOrDefault(g => g.Email.ToLower() == email.ToLower());
            if (guest == null) return new List<ReservationResponseDto>();

            return _reservations
                .Where(r => r.GuestId == guest.Id)
                .Select(r => MapToResponseDto(r))
                .OrderByDescending(r => r.CheckInDate)
                .ToList();
        }

        // Get reservations by hotel
        public List<ReservationResponseDto> GetReservationsByHotel(int hotelId)
        {
            return _reservations
                .Where(r => r.HotelId == hotelId)
                .Select(r => MapToResponseDto(r))
                .ToList();
        }

        // Get reservations by date range
        public List<ReservationResponseDto> GetReservationsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _reservations
                .Where(r => r.CheckInDate >= startDate && r.CheckOutDate <= endDate)
                .Select(r => MapToResponseDto(r))
                .ToList();
        }

        // Get today's arrivals
        public List<ReservationResponseDto> GetTodayArrivals()
        {
            var today = DateTime.Today;
            return _reservations
                .Where(r => r.CheckInDate.Date == today && r.Status != "Cancelled")
                .Select(r => MapToResponseDto(r))
                .ToList();
        }

        // Get today's departures
        public List<ReservationResponseDto> GetTodayDepartures()
        {
            var today = DateTime.Today;
            return _reservations
                .Where(r => r.CheckOutDate.Date == today && r.Status == "CheckedIn")
                .Select(r => MapToResponseDto(r))
                .ToList();
        }

        // Create new reservation
        public ReservationResponseDto CreateReservation(CreateReservationDto dto)
        {
            // Validate dates
            if (dto.CheckInDate >= dto.CheckOutDate)
                throw new Exception("Check-out date must be after check-in date");

            if (dto.CheckInDate < DateTime.Today)
                throw new Exception("Cannot make reservation for past dates");

            // Check room availability
            var room = _rooms.FirstOrDefault(r => r.Id == dto.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            var isAvailable = IsRoomAvailable(dto.RoomId, dto.CheckInDate, dto.CheckOutDate);
            if (!isAvailable)
                throw new Exception("Room is not available for selected dates");

            // Find or create guest
            var guest = _guests.FirstOrDefault(g => g.Email.ToLower() == dto.GuestEmail.ToLower());
            if (guest == null)
            {
                guest = MockData.CreateNewGuest(dto);
                _guests.Add(guest);
            }

            // Create reservation
            var reservation = MockData.CreateNewReservation(dto, dto.RoomId, room.HotelId, (decimal)room.Price);
            reservation.GuestId = guest.Id;
            _reservations.Add(reservation);

            // Add history
            var history = MockData.CreateHistory(
                reservation.Id, 
                "Created", 
                $"Reservation created via {dto.Source}", 
                $"{dto.GuestFirstName} {dto.GuestLastName}"
            );
            _histories.Add(history);

            return MapToResponseDto(reservation);
        }

        // Update reservation
        public ReservationResponseDto UpdateReservation(int id, UpdateReservationDto dto)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null)
                throw new Exception("Reservation not found");

            if (reservation.Status == "Cancelled")
                throw new Exception("Cannot modify cancelled reservation");

            if (reservation.Status == "CheckedIn")
                throw new Exception("Cannot modify checked-in reservation");

            var oldValues = $"CheckIn:{reservation.CheckInDate}, CheckOut:{reservation.CheckOutDate}, Guests:{reservation.GuestCount}";
            var changes = new List<string>();

            if (dto.CheckInDate.HasValue && dto.CheckInDate != reservation.CheckInDate)
            {
                reservation.CheckInDate = dto.CheckInDate.Value;
                changes.Add($"CheckIn changed to {dto.CheckInDate.Value}");
            }

            if (dto.CheckOutDate.HasValue && dto.CheckOutDate != reservation.CheckOutDate)
            {
                reservation.CheckOutDate = dto.CheckOutDate.Value;
                changes.Add($"CheckOut changed to {dto.CheckOutDate.Value}");
            }

            if (dto.GuestCount.HasValue && dto.GuestCount != reservation.GuestCount)
            {
                reservation.GuestCount = dto.GuestCount.Value;
                changes.Add($"GuestCount changed to {dto.GuestCount.Value}");
            }

            if (!string.IsNullOrEmpty(dto.SpecialRequests))
            {
                reservation.SpecialRequests = dto.SpecialRequests;
                changes.Add($"SpecialRequests updated");
            }

            reservation.ModifiedAt = DateTime.Now;

            var history = MockData.CreateHistory(
                reservation.Id,
                "Modified",
                $"Reservation modified: {string.Join(", ", changes)}. Old: {oldValues}",
                "System"
            );
            _histories.Add(history);

            return MapToResponseDto(reservation);
        }

        // Cancel reservation
        public bool CancelReservation(int id, string reason)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null)
                throw new Exception("Reservation not found");

            if (reservation.Status == "Cancelled")
                throw new Exception("Reservation already cancelled");

            if (reservation.Status == "CheckedIn")
                throw new Exception("Cannot cancel checked-in reservation");

            if (reservation.CheckInDate <= DateTime.Today)
                throw new Exception("Cannot cancel reservation that has already started");

            reservation.Status = "Cancelled";
            reservation.CancellationReason = reason;
            reservation.ModifiedAt = DateTime.Now;

            var history = MockData.CreateHistory(
                reservation.Id,
                "Cancelled",
                $"Reservation cancelled. Reason: {reason}",
                "System"
            );
            _histories.Add(history);

            return true;
        }

        // Check-in
        public CheckInOutResponseDto CheckIn(CheckInDto dto)
        {
            var reservation = _reservations.FirstOrDefault(r => r.ReservationNumber == dto.ReservationNumber);
            if (reservation == null)
                throw new Exception("Reservation not found");

            var guest = _guests.FirstOrDefault(g => g.Id == reservation.GuestId);
            if (guest.Email.ToLower() != dto.GuestEmail.ToLower())
                throw new Exception("Guest email does not match reservation");

            if (reservation.Status == "Cancelled")
                throw new Exception("Cannot check-in cancelled reservation");

            if (reservation.Status == "CheckedIn")
                throw new Exception("Already checked in");

            if (reservation.Status == "CheckedOut")
                throw new Exception("Already checked out");

            if (reservation.CheckInDate.Date > DateTime.Today)
                throw new Exception("Cannot check-in before check-in date");

            reservation.Status = "CheckedIn";
            reservation.ActualCheckIn = DateTime.Now;

            var history = MockData.CreateHistory(
                reservation.Id,
                "CheckedIn",
                $"Guest checked in. Passport: {dto.PassportNumber}",
                "Front Desk"
            );
            _histories.Add(history);

            // Generate digital key (mock)
            var digitalKey = $"DK-{reservation.ReservationNumber}-{Guid.NewGuid().ToString().Substring(0, 8)}";

            return new CheckInOutResponseDto
            {
                Success = true,
                Message = "Check-in successful",
                Reservation = MapToResponseDto(reservation),
                DigitalKey = digitalKey
            };
        }

        // Check-out
        public CheckInOutResponseDto CheckOut(CheckOutDto dto)
        {
            var reservation = _reservations.FirstOrDefault(r => r.ReservationNumber == dto.ReservationNumber);
            if (reservation == null)
                throw new Exception("Reservation not found");

            var guest = _guests.FirstOrDefault(g => g.Id == reservation.GuestId);
            if (guest.Email.ToLower() != dto.GuestEmail.ToLower())
                throw new Exception("Guest email does not match reservation");

            if (reservation.Status != "CheckedIn")
                throw new Exception("Cannot check-out reservation that is not checked in");

            reservation.Status = "CheckedOut";
            reservation.ActualCheckOut = DateTime.Now;

            if (dto.ExtraCharges.HasValue && dto.ExtraCharges > 0)
            {
                reservation.TotalPrice += dto.ExtraCharges.Value;
                reservation.PaidAmount = reservation.TotalPrice;
                
                var history = MockData.CreateHistory(
                    reservation.Id,
                    "ExtraCharges",
                    $"Extra charges added: {dto.ExtraCharges} - {dto.ExtraChargesDescription}",
                    "Front Desk"
                );
                _histories.Add(history);
            }

            // Update guest stats
            guest.TotalStays++;
            guest.TotalSpent += reservation.TotalPrice;

            var history2 = MockData.CreateHistory(
                reservation.Id,
                "CheckedOut",
                $"Guest checked out. Total: {reservation.TotalPrice}",
                "Front Desk"
            );
            _histories.Add(history2);

            return new CheckInOutResponseDto
            {
                Success = true,
                Message = "Check-out successful",
                Reservation = MapToResponseDto(reservation),
                TotalExtraCharges = dto.ExtraCharges
            };
        }

        // Check room availability
        public bool IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var overlappingReservations = _reservations.Any(r =>
                r.RoomId == roomId &&
                r.Status != "Cancelled" &&
                !(r.CheckOutDate <= checkIn || r.CheckInDate >= checkOut)
            );
            return !overlappingReservations;
        }

        // Get reservation history
        public List<ReservationHistory> GetReservationHistory(int reservationId)
        {
            return _histories
                .Where(h => h.ReservationId == reservationId)
                .OrderBy(h => h.PerformedAt)
                .ToList();
        }

        // Get statistics
        public object GetStatistics(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _reservations.AsQueryable();
            
            if (startDate.HasValue)
                query = query.Where(r => r.CreatedAt >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(r => r.CreatedAt <= endDate.Value);

            var reservations = query.ToList();

            return new
            {
                TotalReservations = reservations.Count,
                TotalRevenue = reservations.Sum(r => r.TotalPrice),
                AverageBookingValue = reservations.Any() ? reservations.Average(r => r.TotalPrice) : 0,
                ByStatus = reservations.GroupBy(r => r.Status).Select(g => new { Status = g.Key, Count = g.Count() }),
                BySource = reservations.GroupBy(r => r.Source).Select(g => new { Source = g.Key, Count = g.Count() }),
                ConfirmedReservations = reservations.Count(r => r.Status == "Confirmed"),
                CheckedInToday = reservations.Count(r => r.Status == "CheckedIn" && r.ActualCheckIn.Date == DateTime.Today),
                CheckedOutToday = reservations.Count(r => r.Status == "CheckedOut" && r.ActualCheckOut.Date == DateTime.Today),
                CancelledReservations = reservations.Count(r => r.Status == "Cancelled"),
                UpcomingArrivals = reservations.Count(r => r.CheckInDate.Date > DateTime.Today && r.Status == "Confirmed"),
                OccupancyRate = CalculateOccupancyRate(DateTime.Today)
            };
        }

        private decimal CalculateOccupancyRate(DateTime date)
        {
            var totalRooms = _rooms.Count;
            var occupiedRooms = _reservations.Count(r =>
                r.Status == "CheckedIn" &&
                r.CheckInDate.Date <= date &&
                r.CheckOutDate.Date >= date
            );
            return totalRooms > 0 ? Math.Round((decimal)occupiedRooms / totalRooms * 100, 2) : 0;
        }

        private ReservationResponseDto MapToResponseDto(Reservation reservation)
        {
            var guest = _guests.FirstOrDefault(g => g.Id == reservation.GuestId);
            var room = _rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
            var nightCount = (reservation.CheckOutDate - reservation.CheckInDate).Days;
            var canCancel = reservation.Status == "Confirmed" && reservation.CheckInDate > DateTime.Today;
            var canModify = reservation.Status == "Confirmed" && reservation.CheckInDate > DateTime.Today.AddDays(1);

            return new ReservationResponseDto
            {
                Id = reservation.Id,
                ReservationNumber = reservation.ReservationNumber,
                GuestName = guest != null ? $"{guest.FirstName} {guest.LastName}" : "Unknown",
                GuestEmail = guest?.Email,
                HotelName = room?.HotelName,
                RoomNumber = room?.RoomNumber,
                RoomType = room?.RoomType,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                NightCount = nightCount,
                GuestCount = reservation.GuestCount,
                TotalPrice = reservation.TotalPrice,
                Status = reservation.Status,
                PaymentStatus = reservation.PaymentStatus,
                SpecialRequests = reservation.SpecialRequests,
                CreatedAt = reservation.CreatedAt,
                CanCancel = canCancel,
                CanModify = canModify
            };
        }
    }
}