using ReservationSystem.API.DTOs;
using ReservationSystem.API.Models;
using System.Collections.Generic;

namespace ReservationSystem.API.Data
{
    public static class MockData
    {
        private static int _nextReservationId = 100;
        private static int _nextGuestId = 50;
        private static int _nextHistoryId = 500;

        public static List<Guest> GetGuests()
        {
            return new List<Guest>
            {
                new Guest 
                { 
                    Id = 1, FirstName = "Ahmet", LastName = "Yılmaz", Email = "ahmet.yilmaz@email.com",
                    Phone = "+905551234567", Country = "Turkey", PassportNumber = "TR123456",
                    DateOfBirth = new DateTime(1985, 5, 15), Gender = "Male",
                    Address = "Levent Mah., Istanbul", City = "Istanbul", PostalCode = "34330",
                    CreatedAt = new DateTime(2023, 1, 10), TotalStays = 5, TotalSpent = 2500,
                    Preferences = "High floor, non-smoking"
                },
                new Guest 
                { 
                    Id = 2, FirstName = "Ayşe", LastName = "Demir", Email = "ayse.demir@email.com",
                    Phone = "+905559876543", Country = "Turkey", PassportNumber = "TR789012",
                    DateOfBirth = new DateTime(1990, 8, 22), Gender = "Female",
                    Address = "Alsancak Mah., Izmir", City = "Izmir", PostalCode = "35220",
                    CreatedAt = new DateTime(2023, 3, 15), TotalStays = 3, TotalSpent = 1800,
                    Preferences = "Sea view"
                },
                new Guest 
                { 
                    Id = 3, FirstName = "John", LastName = "Smith", Email = "john.smith@email.com",
                    Phone = "+447912345678", Country = "UK", PassportNumber = "GB987654",
                    DateOfBirth = new DateTime(1980, 12, 5), Gender = "Male",
                    Address = "London, UK", City = "London", PostalCode = "SW1A1AA",
                    CreatedAt = new DateTime(2022, 6, 20), TotalStays = 8, TotalSpent = 8000,
                    Preferences = "VIP service"
                }
            };
        }

        public static List<Reservation> GetReservations()
        {
            var guests = GetGuests();
            var rooms = GetRooms();

            return new List<Reservation>
            {
                new Reservation 
                { 
                    Id = 1, ReservationNumber = "RES-2024-001", GuestId = 1, RoomId = 1, HotelId = 1,
                    CheckInDate = new DateTime(2024, 6, 15), CheckOutDate = new DateTime(2024, 6, 18),
                    ActualCheckIn = new DateTime(2024, 6, 15, 14, 30, 0),
                    GuestCount = 2, ChildCount = 0, TotalPrice = 450, PaidAmount = 450,
                    Status = "CheckedOut", PaymentStatus = "Paid", PaymentMethod = "CreditCard",
                    SpecialRequests = "High floor", CreatedAt = new DateTime(2024, 5, 20),
                    Source = "Web"
                },
                new Reservation 
                { 
                    Id = 2, ReservationNumber = "RES-2024-002", GuestId = 2, RoomId = 4, HotelId = 2,
                    CheckInDate = new DateTime(2024, 7, 10), CheckOutDate = new DateTime(2024, 7, 15),
                    GuestCount = 2, ChildCount = 1, TotalPrice = 600, PaidAmount = 300,
                    Status = "Confirmed", PaymentStatus = "Partial", PaymentMethod = "PayPal",
                    SpecialRequests = "", CreatedAt = new DateTime(2024, 6, 1),
                    Source = "Mobile"
                },
                new Reservation 
                { 
                    Id = 3, ReservationNumber = "RES-2024-003", GuestId = 3, RoomId = 7, HotelId = 3,
                    CheckInDate = new DateTime(2024, 8, 1), CheckOutDate = new DateTime(2024, 8, 5),
                    GuestCount = 2, ChildCount = 0, TotalPrice = 2000, PaidAmount = 2000,
                    Status = "Confirmed", PaymentStatus = "Paid", PaymentMethod = "CreditCard",
                    SpecialRequests = "Butler service", CreatedAt = new DateTime(2024, 6, 10),
                    Source = "CallCenter"
                },
                new Reservation 
                { 
                    Id = 4, ReservationNumber = "RES-2024-004", GuestId = 1, RoomId = 2, HotelId = 1,
                    CheckInDate = new DateTime(2024, 6, 20), CheckOutDate = new DateTime(2024, 6, 22),
                    GuestCount = 2, ChildCount = 0, TotalPrice = 500, PaidAmount = 0,
                    Status = "Pending", PaymentStatus = "Pending", PaymentMethod = "BankTransfer",
                    SpecialRequests = "", CreatedAt = new DateTime(2024, 6, 15),
                    Source = "Web"
                }
            };
        }

        public static List<ReservationHistory> GetReservationHistories()
        {
            return new List<ReservationHistory>
            {
                new ReservationHistory 
                { 
                    Id = 1, ReservationId = 1, Action = "Created", 
                    Description = "Reservation created via Web", 
                    PerformedBy = "Ahmet Yılmaz", PerformedAt = new DateTime(2024, 5, 20, 10, 30, 0)
                },
                new ReservationHistory 
                { 
                    Id = 2, ReservationId = 1, Action = "CheckedIn", 
                    Description = "Guest checked in", 
                    PerformedBy = "Front Desk", PerformedAt = new DateTime(2024, 6, 15, 14, 30, 0)
                },
                new ReservationHistory 
                { 
                    Id = 3, ReservationId = 1, Action = "CheckedOut", 
                    Description = "Guest checked out", 
                    PerformedBy = "Front Desk", PerformedAt = new DateTime(2024, 6, 18, 11, 0, 0)
                }
            };
        }

        private static List<dynamic> GetRooms()
        {
            return new List<dynamic>
            {
                new { Id = 1, HotelId = 1, HotelName = "Marriott Istanbul", RoomNumber = "101", RoomType = "Standard" },
                new { Id = 2, HotelId = 1, HotelName = "Marriott Istanbul", RoomNumber = "202", RoomType = "Deluxe" },
                new { Id = 4, HotelId = 2, HotelName = "Hilton Izmir", RoomNumber = "110", RoomType = "Standard" },
                new { Id = 7, HotelId = 3, HotelName = "Sofitel Bodrum", RoomNumber = "501", RoomType = "Suite" }
            };
        }

        public static Reservation CreateNewReservation(CreateReservationDto dto, int roomId, int hotelId, decimal price)
        {
            var nightCount = (dto.CheckOutDate - dto.CheckInDate).Days;
            var totalPrice = price * nightCount;

            return new Reservation
            {
                Id = _nextReservationId++,
                ReservationNumber = $"RES-{DateTime.Now.Year}-{_nextReservationId:D4}",
                GuestId = _nextGuestId,
                RoomId = roomId,
                HotelId = hotelId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                GuestCount = dto.GuestCount,
                ChildCount = dto.ChildCount,
                TotalPrice = totalPrice,
                PaidAmount = 0,
                Status = "Pending",
                PaymentStatus = "Pending",
                PaymentMethod = dto.PaymentMethod,
                SpecialRequests = dto.SpecialRequests,
                CreatedAt = DateTime.Now,
                Source = dto.Source
            };
        }

        public static Guest CreateNewGuest(CreateReservationDto dto)
        {
            return new Guest
            {
                Id = _nextGuestId++,
                FirstName = dto.GuestFirstName,
                LastName = dto.GuestLastName,
                Email = dto.GuestEmail,
                Phone = dto.GuestPhone,
                Country = "Unknown",
                CreatedAt = DateTime.Now,
                TotalStays = 0,
                TotalSpent = 0
            };
        }

        public static ReservationHistory CreateHistory(int reservationId, string action, string description, string performedBy)
        {
            return new ReservationHistory
            {
                Id = _nextHistoryId++,
                ReservationId = reservationId,
                Action = action,
                Description = description,
                PerformedBy = performedBy,
                PerformedAt = DateTime.Now
            };
        }
    }
}