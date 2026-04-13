using CRM_Loyalty.API.DTOs;
using CRM_Loyalty.API.Models;
using System.Collections.Generic;

namespace CRM_Loyalty.API.Data
{
    public static class MockData
    {
        private static int _nextCustomerId = 100;
        private static int _nextTransactionId = 1000;
        private static int _nextPreferenceId = 500;

        public static List<MembershipLevel> GetMembershipLevels()
        {
            return new List<MembershipLevel>
            {
                new MembershipLevel 
                { 
                    Id = 1, Name = "Bronze", MinPoints = 0, MaxPoints = 999, 
                    PointsMultiplier = 1.0m, DiscountRate = 0m, FreeUpgradePerYear = 0,
                    LateCheckoutHours = 0, EarlyCheckinHours = 0, FreeBreakfast = false,
                    AirportTransfer = false, LoungeAccess = false,
                    Color = "#cd7f32", Icon = "🥉"
                },
                new MembershipLevel 
                { 
                    Id = 2, Name = "Silver", MinPoints = 1000, MaxPoints = 4999, 
                    PointsMultiplier = 1.1m, DiscountRate = 5m, FreeUpgradePerYear = 1,
                    LateCheckoutHours = 1, EarlyCheckinHours = 1, FreeBreakfast = false,
                    AirportTransfer = false, LoungeAccess = false,
                    Color = "#c0c0c0", Icon = "🥈"
                },
                new MembershipLevel 
                { 
                    Id = 3, Name = "Gold", MinPoints = 5000, MaxPoints = 14999, 
                    PointsMultiplier = 1.25m, DiscountRate = 10m, FreeUpgradePerYear = 2,
                    LateCheckoutHours = 2, EarlyCheckinHours = 2, FreeBreakfast = true,
                    AirportTransfer = false, LoungeAccess = false,
                    Color = "#ffd700", Icon = "🥇"
                },
                new MembershipLevel 
                { 
                    Id = 4, Name = "Platinum", MinPoints = 15000, MaxPoints = 49999, 
                    PointsMultiplier = 1.5m, DiscountRate = 15m, FreeUpgradePerYear = 3,
                    LateCheckoutHours = 3, EarlyCheckinHours = 3, FreeBreakfast = true,
                    AirportTransfer = true, LoungeAccess = true,
                    Color = "#e5e4e2", Icon = "💎"
                },
                new MembershipLevel 
                { 
                    Id = 5, Name = "Diamond", MinPoints = 50000, MaxPoints = int.MaxValue, 
                    PointsMultiplier = 2.0m, DiscountRate = 20m, FreeUpgradePerYear = 5,
                    LateCheckoutHours = 4, EarlyCheckinHours = 4, FreeBreakfast = true,
                    AirportTransfer = true, LoungeAccess = true,
                    Color = "#b9f2ff", Icon = "👑"
                }
            };
        }

        public static List<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                new Customer 
                { 
                    Id = 1, CustomerNumber = "CUST-00001", Email = "ahmet.yilmaz@email.com",
                    FirstName = "Ahmet", LastName = "Yılmaz", PhoneNumber = "+905551234567",
                    Country = "Turkey", City = "İstanbul", Address = "Levent Mah., No:123",
                    DateOfBirth = new DateTime(1985, 5, 15), Gender = "Male",
                    RegistrationDate = new DateTime(2022, 1, 10), LastActivityDate = new DateTime(2024, 6, 1),
                    Status = "Active", TotalStays = 12, TotalNights = 45, TotalSpent = 6750,
                    LoyaltyPoints = 12500, MembershipLevel = "Gold", PreferredLanguage = "Turkish",
                    Notes = "VIP customer, prefers high floor", ProfileImageUrl = "ahmet.jpg"
                },
                new Customer 
                { 
                    Id = 2, CustomerNumber = "CUST-00002", Email = "ayse.demir@email.com",
                    FirstName = "Ayşe", LastName = "Demir", PhoneNumber = "+905559876543",
                    Country = "Turkey", City = "İzmir", Address = "Alsancak Mah., No:45",
                    DateOfBirth = new DateTime(1990, 8, 22), Gender = "Female",
                    RegistrationDate = new DateTime(2022, 3, 15), LastActivityDate = new DateTime(2024, 5, 20),
                    Status = "Active", TotalStays = 5, TotalNights = 18, TotalSpent = 2700,
                    LoyaltyPoints = 4800, MembershipLevel = "Silver", PreferredLanguage = "Turkish",
                    Notes = "Prefers sea view rooms", ProfileImageUrl = "ayse.jpg"
                },
                new Customer 
                { 
                    Id = 3, CustomerNumber = "CUST-00003", Email = "john.smith@email.com",
                    FirstName = "John", LastName = "Smith", PhoneNumber = "+447912345678",
                    Country = "UK", City = "London", Address = "Mayfair, London",
                    DateOfBirth = new DateTime(1978, 12, 5), Gender = "Male",
                    RegistrationDate = new DateTime(2021, 6, 20), LastActivityDate = new DateTime(2024, 6, 10),
                    Status = "Active", TotalStays = 25, TotalNights = 98, TotalSpent = 24500,
                    LoyaltyPoints = 52000, MembershipLevel = "Diamond", PreferredLanguage = "English",
                    Notes = "Corporate client, requires airport transfer", ProfileImageUrl = "john.jpg"
                },
                new Customer 
                { 
                    Id = 4, CustomerNumber = "CUST-00004", Email = "maria.garcia@email.com",
                    FirstName = "Maria", LastName = "Garcia", PhoneNumber = "+34612345678",
                    Country = "Spain", City = "Barcelona", Address = "Rambla de Catalunya",
                    DateOfBirth = new DateTime(1995, 3, 10), Gender = "Female",
                    RegistrationDate = new DateTime(2023, 1, 5), LastActivityDate = new DateTime(2024, 5, 30),
                    Status = "Active", TotalStays = 3, TotalNights = 10, TotalSpent = 1500,
                    LoyaltyPoints = 1250, MembershipLevel = "Bronze", PreferredLanguage = "Spanish",
                    Notes = "", ProfileImageUrl = "maria.jpg"
                },
                new Customer 
                { 
                    Id = 5, CustomerNumber = "CUST-00005", Email = "mehmet.kaya@email.com",
                    FirstName = "Mehmet", LastName = "Kaya", PhoneNumber = "+905321234567",
                    Country = "Turkey", City = "Antalya", Address = "Lara Cad., No:67",
                    DateOfBirth = new DateTime(1988, 7, 18), Gender = "Male",
                    RegistrationDate = new DateTime(2023, 6, 10), LastActivityDate = new DateTime(2024, 6, 5),
                    Status = "Active", TotalStays = 8, TotalNights = 30, TotalSpent = 4500,
                    LoyaltyPoints = 8200, MembershipLevel = "Gold", PreferredLanguage = "Turkish",
                    Notes = "Likes golf activities", ProfileImageUrl = "mehmet.jpg"
                }
            };
        }

        public static List<LoyaltyTransaction> GetLoyaltyTransactions()
        {
            var transactions = new List<LoyaltyTransaction>();
            var random = new Random();
            var customers = GetCustomers();

            foreach (var customer in customers)
            {
                var points = customer.LoyaltyPoints;
                var currentPoints = 0;

                // Earn transactions
                for (int i = 0; i < customer.TotalStays; i++)
                {
                    var earnedPoints = random.Next(100, 1000);
                    transactions.Add(new LoyaltyTransaction
                    {
                        Id = _nextTransactionId++,
                        CustomerId = customer.Id,
                        TransactionType = "Earn",
                        Points = earnedPoints,
                        PointsBefore = currentPoints,
                        PointsAfter = currentPoints + earnedPoints,
                        Source = "Reservation",
                        SourceId = $"RES-{2023 + i}-{random.Next(100, 999)}",
                        Description = $"Points earned from stay at hotel",
                        TransactionDate = DateTime.Now.AddDays(-random.Next(30, 365)),
                        ExpiryDate = DateTime.Now.AddYears(1).AddDays(-random.Next(30, 365))
                    });
                    currentPoints += earnedPoints;
                }

                // Bonus transactions (birthday, etc.)
                var bonusPoints = random.Next(50, 500);
                transactions.Add(new LoyaltyTransaction
                {
                    Id = _nextTransactionId++,
                    CustomerId = customer.Id,
                    TransactionType = "Bonus",
                    Points = bonusPoints,
                    PointsBefore = currentPoints,
                    PointsAfter = currentPoints + bonusPoints,
                    Source = "Birthday",
                    SourceId = null,
                    Description = "Birthday bonus points",
                    TransactionDate = customer.DateOfBirth.AddYears(DateTime.Now.Year - customer.DateOfBirth.Year),
                    ExpiryDate = DateTime.Now.AddYears(1)
                });
                currentPoints += bonusPoints;
            }

            return transactions;
        }

        public static List<CustomerPreference> GetCustomerPreferences()
        {
            return new List<CustomerPreference>
            {
                new CustomerPreference 
                { 
                    Id = 1, CustomerId = 1, PreferenceType = "Floor", 
                    PreferenceValue = "High", Description = "Prefers high floor rooms",
                    CreatedAt = new DateTime(2023, 1, 10), UpdatedAt = null
                },
                new CustomerPreference 
                { 
                    Id = 2, CustomerId = 1, PreferenceType = "Pillow", 
                    PreferenceValue = "Memory Foam", Description = "Memory foam pillow preferred",
                    CreatedAt = new DateTime(2023, 1, 10), UpdatedAt = new DateTime(2024, 1, 15)
                },
                new CustomerPreference 
                { 
                    Id = 3, CustomerId = 1, PreferenceType = "Newspaper", 
                    PreferenceValue = "Hürriyet", Description = "Daily newspaper preference",
                    CreatedAt = new DateTime(2023, 2, 5), UpdatedAt = null
                },
                new CustomerPreference 
                { 
                    Id = 4, CustomerId = 2, PreferenceType = "View", 
                    PreferenceValue = "Sea View", Description = "Prefers sea view rooms",
                    CreatedAt = new DateTime(2023, 4, 10), UpdatedAt = null
                },
                new CustomerPreference 
                { 
                    Id = 5, CustomerId = 2, PreferenceType = "Dietary", 
                    PreferenceValue = "Vegetarian", Description = "Vegetarian meal preference",
                    CreatedAt = new DateTime(2023, 4, 10), UpdatedAt = new DateTime(2024, 2, 20)
                },
                new CustomerPreference 
                { 
                    Id = 6, CustomerId = 3, PreferenceType = "Transfer", 
                    PreferenceValue = "Airport Transfer", Description = "Requires airport transfer",
                    CreatedAt = new DateTime(2022, 7, 1), UpdatedAt = null
                },
                new CustomerPreference 
                { 
                    Id = 7, CustomerId = 3, PreferenceType = "Room Service", 
                    PreferenceValue = "24/7", Description = "Prefers 24/7 room service",
                    CreatedAt = new DateTime(2022, 8, 15), UpdatedAt = null
                },
                new CustomerPreference 
                { 
                    Id = 8, CustomerId = 5, PreferenceType = "Activities", 
                    PreferenceValue = "Golf", Description = "Interested in golf activities",
                    CreatedAt = new DateTime(2023, 7, 10), UpdatedAt = null
                }
            };
        }

        public static Customer CreateNewCustomer(CustomerDto dto)
        {
            return new Customer
            {
                Id = _nextCustomerId++,
                CustomerNumber = $"CUST-{_nextCustomerId:D5}",
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Country = dto.Country,
                City = dto.City,
                RegistrationDate = DateTime.Now,
                LastActivityDate = DateTime.Now,
                Status = "Active",
                TotalStays = 0,
                TotalNights = 0,
                TotalSpent = 0,
                LoyaltyPoints = 100, // Welcome bonus
                MembershipLevel = "Bronze",
                PreferredLanguage = "Turkish"
            };
        }
    }
}