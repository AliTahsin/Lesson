using System;
using System.Collections.Generic;
using HotelManagement.API.Models;
using ReservationSystem.API.Models;
using PaymentInvoice.API.Models;
using Auth.API.Models;

namespace Backend.Tests.TestData
{
    public static class MockData
    {
        private static readonly Random _random = new();

        public static List<Hotel> GetTestHotels()
        {
            return new List<Hotel>
            {
                new Hotel
                {
                    Id = 1,
                    Name = "Test Hotel Istanbul",
                    BrandId = 1,
                    City = "İstanbul",
                    Country = "Turkey",
                    Address = "Test Address 1",
                    StarRating = 5,
                    Phone = "+905551234567",
                    Email = "test@hotel.com",
                    Description = "Test hotel description",
                    TotalRooms = 100,
                    Status = "Active",
                    Amenities = new List<string> { "WiFi", "Pool", "Spa" },
                    Images = new List<string> { "test1.jpg" },
                    OpeningDate = new DateTime(2020, 1, 1)
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Test Hotel Ankara",
                    BrandId = 2,
                    City = "Ankara",
                    Country = "Turkey",
                    Address = "Test Address 2",
                    StarRating = 4,
                    Phone = "+905559876543",
                    Email = "test2@hotel.com",
                    Description = "Test hotel description 2",
                    TotalRooms = 80,
                    Status = "Active",
                    Amenities = new List<string> { "WiFi", "Restaurant" },
                    Images = new List<string> { "test2.jpg" },
                    OpeningDate = new DateTime(2021, 6, 15)
                }
            };
        }

        public static List<Reservation> GetTestReservations()
        {
            return new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    ReservationNumber = "RES-TEST-001",
                    GuestId = 1,
                    RoomId = 101,
                    HotelId = 1,
                    CheckInDate = new DateTime(2024, 12, 1),
                    CheckOutDate = new DateTime(2024, 12, 5),
                    GuestCount = 2,
                    TotalPrice = 600,
                    Status = "Confirmed",
                    PaymentStatus = "Paid",
                    PaymentMethod = "CreditCard",
                    CreatedAt = new DateTime(2024, 11, 15)
                },
                new Reservation
                {
                    Id = 2,
                    ReservationNumber = "RES-TEST-002",
                    GuestId = 2,
                    RoomId = 102,
                    HotelId = 1,
                    CheckInDate = new DateTime(2024, 12, 10),
                    CheckOutDate = new DateTime(2024, 12, 12),
                    GuestCount = 1,
                    TotalPrice = 300,
                    Status = "Pending",
                    PaymentStatus = "Pending",
                    PaymentMethod = "PayPal",
                    CreatedAt = new DateTime(2024, 11, 20)
                }
            };
        }

        public static List<Payment> GetTestPayments()
        {
            return new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    PaymentNumber = "PAY-TEST-001",
                    ReservationId = 1,
                    CustomerId = 1,
                    Amount = 600,
                    Currency = "EUR",
                    PaymentMethod = "CreditCard",
                    CardBrand = "Visa",
                    MaskedCardNumber = "**** **** **** 1234",
                    Status = "Success",
                    TransactionId = "TXN-TEST-001",
                    PaymentDate = new DateTime(2024, 11, 15)
                },
                new Payment
                {
                    Id = 2,
                    PaymentNumber = "PAY-TEST-002",
                    ReservationId = 2,
                    CustomerId = 2,
                    Amount = 300,
                    Currency = "EUR",
                    PaymentMethod = "PayPal",
                    Status = "Pending",
                    PaymentDate = new DateTime(2024, 11, 20)
                }
            };
        }

        public static List<User> GetTestUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@test.com",
                    PhoneNumber = "+905551234567",
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    IsActive = true,
                    IsEmailVerified = true,
                    HotelId = 1,
                    RoleIds = new List<int> { 1 },
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new User
                {
                    Id = 2,
                    FirstName = "Test",
                    LastName = "User",
                    Email = "test@test.com",
                    PhoneNumber = "+905559876543",
                    Username = "testuser",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
                    IsActive = true,
                    IsEmailVerified = true,
                    HotelId = 1,
                    RoleIds = new List<int> { 4 },
                    CreatedAt = new DateTime(2024, 6, 1)
                }
            };
        }

        public static List<Role> GetTestRoles()
        {
            return new List<Role>
            {
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    Description = "Administrator role",
                    Level = "SuperAdmin",
                    PermissionIds = new List<int> { 1, 2, 3, 4, 5 },
                    IsDefault = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Role
                {
                    Id = 2,
                    Name = "Manager",
                    Description = "Manager role",
                    Level = "Admin",
                    PermissionIds = new List<int> { 1, 2, 3 },
                    IsDefault = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Role
                {
                    Id = 3,
                    Name = "Staff",
                    Description = "Staff role",
                    Level = "Staff",
                    PermissionIds = new List<int> { 1 },
                    IsDefault = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Role
                {
                    Id = 4,
                    Name = "Guest",
                    Description = "Guest role",
                    Level = "Guest",
                    PermissionIds = new List<int> { 1 },
                    IsDefault = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            };
        }

        public static List<Permission> GetTestPermissions()
        {
            return new List<Permission>
            {
                new Permission
                {
                    Id = 1,
                    Name = "View Profile",
                    Code = "user:view",
                    Category = "User",
                    Description = "Can view own profile",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Permission
                {
                    Id = 2,
                    Name = "Edit Profile",
                    Code = "user:edit",
                    Category = "User",
                    Description = "Can edit own profile",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Permission
                {
                    Id = 3,
                    Name = "Manage Users",
                    Code = "user:manage",
                    Category = "User",
                    Description = "Can manage all users",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Permission
                {
                    Id = 4,
                    Name = "View Reservations",
                    Code = "reservation:view",
                    Category = "Reservation",
                    Description = "Can view reservations",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Permission
                {
                    Id = 5,
                    Name = "Manage Reservations",
                    Code = "reservation:manage",
                    Category = "Reservation",
                    Description = "Can manage reservations",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            };
        }

        public static List<Room> GetTestRooms()
        {
            return new List<Room>
            {
                new Room
                {
                    Id = 101,
                    HotelId = 1,
                    RoomNumber = "101",
                    RoomType = "Standard",
                    BasePrice = 150,
                    Capacity = 2,
                    IsAvailable = true,
                    Status = "Available"
                },
                new Room
                {
                    Id = 102,
                    HotelId = 1,
                    RoomNumber = "102",
                    RoomType = "Deluxe",
                    BasePrice = 250,
                    Capacity = 3,
                    IsAvailable = true,
                    Status = "Available"
                }
            };
        }

        public static List<Guest> GetTestGuests()
        {
            return new List<Guest>
            {
                new Guest
                {
                    Id = 1,
                    FirstName = "Test",
                    LastName = "Guest",
                    Email = "guest@test.com",
                    Phone = "+905551234567",
                    Country = "Turkey",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Guest
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@test.com",
                    Phone = "+905559876543",
                    Country = "USA",
                    CreatedAt = new DateTime(2024, 6, 1)
                }
            };
        }
    }
}