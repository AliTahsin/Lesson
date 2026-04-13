using CRM_Loyalty.API.Models;
using CRM_Loyalty.API.DTOs;
using CRM_Loyalty.API.Data;

namespace CRM_Loyalty.API.Services
{
    public class LoyaltyService
    {
        private readonly List<Customer> _customers;
        private readonly List<MembershipLevel> _membershipLevels;
        private readonly List<LoyaltyTransaction> _transactions;
        private readonly List<CustomerPreference> _preferences;

        public LoyaltyService()
        {
            _customers = MockData.GetCustomers();
            _membershipLevels = MockData.GetMembershipLevels();
            _transactions = MockData.GetLoyaltyTransactions();
            _preferences = MockData.GetCustomerPreferences();
        }

        // Customer CRUD
        public List<CustomerDto> GetAllCustomers()
        {
            return _customers.Select(c => MapToDto(c)).ToList();
        }

        public CustomerDto GetCustomerById(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            return customer != null ? MapToDto(customer) : null;
        }

        public CustomerDto GetCustomerByEmail(string email)
        {
            var customer = _customers.FirstOrDefault(c => c.Email.ToLower() == email.ToLower());
            return customer != null ? MapToDto(customer) : null;
        }

        public CustomerDto GetCustomerByNumber(string customerNumber)
        {
            var customer = _customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
            return customer != null ? MapToDto(customer) : null;
        }

        public CustomerDto CreateCustomer(CustomerDto dto)
        {
            var existing = _customers.FirstOrDefault(c => c.Email.ToLower() == dto.Email.ToLower());
            if (existing != null)
                throw new Exception("Customer with this email already exists");

            var customer = MockData.CreateNewCustomer(dto);
            _customers.Add(customer);

            // Add welcome transaction
            var welcomeTransaction = new LoyaltyTransaction
            {
                Id = _transactions.Max(t => t.Id) + 1,
                CustomerId = customer.Id,
                TransactionType = "Bonus",
                Points = 100,
                PointsBefore = 0,
                PointsAfter = 100,
                Source = "Welcome",
                Description = "Welcome bonus points",
                TransactionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1)
            };
            _transactions.Add(welcomeTransaction);

            return MapToDto(customer);
        }

        public CustomerDto UpdateCustomer(int id, CustomerDto dto)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null) return null;

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.PhoneNumber = dto.PhoneNumber;
            customer.Country = dto.Country;
            customer.City = dto.City;
            customer.Address = dto.Address;
            customer.PreferredLanguage = dto.PreferredLanguage;
            customer.Notes = dto.Notes;

            return MapToDto(customer);
        }

        // Loyalty Operations
        public LoyaltyDto GetLoyaltyInfo(int customerId)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null) return null;

            var level = _membershipLevels.FirstOrDefault(l => l.Name == customer.MembershipLevel);
            var pointsToNextLevel = GetPointsToNextLevel(customer.LoyaltyPoints);
            var nextLevel = GetNextLevel(customer.LoyaltyPoints);

            return new LoyaltyDto
            {
                CustomerId = customer.Id,
                CustomerName = $"{customer.FirstName} {customer.LastName}",
                CurrentPoints = customer.LoyaltyPoints,
                CurrentLevel = customer.MembershipLevel,
                PointsToNextLevel = pointsToNextLevel,
                NextLevel = nextLevel,
                LevelBenefits = level != null ? new LevelBenefitsDto
                {
                    DiscountRate = level.DiscountRate,
                    PointsMultiplier = level.PointsMultiplier,
                    FreeUpgradePerYear = level.FreeUpgradePerYear,
                    LateCheckoutHours = level.LateCheckoutHours,
                    EarlyCheckinHours = level.EarlyCheckinHours,
                    FreeBreakfast = level.FreeBreakfast,
                    AirportTransfer = level.AirportTransfer,
                    LoungeAccess = level.LoungeAccess
                } : null,
                TotalStays = customer.TotalStays,
                TotalNights = customer.TotalNights,
                TotalSpent = customer.TotalSpent
            };
        }

        public LoyaltyTransactionDto AddPoints(int customerId, int points, string source, string description)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null) throw new Exception("Customer not found");

            var pointsBefore = customer.LoyaltyPoints;
            var pointsAfter = pointsBefore + points;

            var transaction = new LoyaltyTransaction
            {
                Id = _transactions.Max(t => t.Id) + 1,
                CustomerId = customerId,
                TransactionType = "Earn",
                Points = points,
                PointsBefore = pointsBefore,
                PointsAfter = pointsAfter,
                Source = source,
                Description = description,
                TransactionDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1)
            };
            _transactions.Add(transaction);

            customer.LoyaltyPoints = pointsAfter;
            customer.LastActivityDate = DateTime.Now;

            // Check for level upgrade
            UpdateMembershipLevel(customer);

            return MapToTransactionDto(transaction);
        }

        public LoyaltyTransactionDto RedeemPoints(int customerId, int points, string description)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null) throw new Exception("Customer not found");

            if (customer.LoyaltyPoints < points)
                throw new Exception("Insufficient points");

            var pointsBefore = customer.LoyaltyPoints;
            var pointsAfter = pointsBefore - points;

            var transaction = new LoyaltyTransaction
            {
                Id = _transactions.Max(t => t.Id) + 1,
                CustomerId = customerId,
                TransactionType = "Redeem",
                Points = points,
                PointsBefore = pointsBefore,
                PointsAfter = pointsAfter,
                Source = "Redeem",
                Description = description,
                TransactionDate = DateTime.Now,
                ExpiryDate = DateTime.Now
            };
            _transactions.Add(transaction);

            customer.LoyaltyPoints = pointsAfter;
            customer.LastActivityDate = DateTime.Now;

            return MapToTransactionDto(transaction);
        }

        public List<LoyaltyTransactionDto> GetTransactionHistory(int customerId)
        {
            return _transactions
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => MapToTransactionDto(t))
                .ToList();
        }

        private void UpdateMembershipLevel(Customer customer)
        {
            var newLevel = _membershipLevels
                .Where(l => customer.LoyaltyPoints >= l.MinPoints && customer.LoyaltyPoints <= l.MaxPoints)
                .OrderByDescending(l => l.MinPoints)
                .FirstOrDefault();

            if (newLevel != null && newLevel.Name != customer.MembershipLevel)
            {
                var oldLevel = customer.MembershipLevel;
                customer.MembershipLevel = newLevel.Name;

                // Add level upgrade transaction
                var upgradeTransaction = new LoyaltyTransaction
                {
                    Id = _transactions.Max(t => t.Id) + 1,
                    CustomerId = customer.Id,
                    TransactionType = "Bonus",
                    Points = 0,
                    PointsBefore = customer.LoyaltyPoints,
                    PointsAfter = customer.LoyaltyPoints,
                    Source = "LevelUp",
                    Description = $"Upgraded from {oldLevel} to {newLevel.Name}",
                    TransactionDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddYears(1)
                };
                _transactions.Add(upgradeTransaction);
            }
        }

        private int GetPointsToNextLevel(int currentPoints)
        {
            var nextLevel = _membershipLevels
                .FirstOrDefault(l => l.MinPoints > currentPoints);
            return nextLevel != null ? nextLevel.MinPoints - currentPoints : 0;
        }

        private string GetNextLevel(int currentPoints)
        {
            var nextLevel = _membershipLevels
                .FirstOrDefault(l => l.MinPoints > currentPoints);
            return nextLevel?.Name ?? "Max Level";
        }

        // Preferences
        public List<PreferenceDto> GetCustomerPreferences(int customerId)
        {
            return _preferences
                .Where(p => p.CustomerId == customerId)
                .Select(p => new PreferenceDto
                {
                    Id = p.Id,
                    PreferenceType = p.PreferenceType,
                    PreferenceValue = p.PreferenceValue,
                    Description = p.Description
                })
                .ToList();
        }

        public PreferenceDto AddPreference(int customerId, PreferenceDto dto)
        {
            var preference = new CustomerPreference
            {
                Id = _preferences.Max(p => p.Id) + 1,
                CustomerId = customerId,
                PreferenceType = dto.PreferenceType,
                PreferenceValue = dto.PreferenceValue,
                Description = dto.Description,
                CreatedAt = DateTime.Now
            };
            _preferences.Add(preference);

            dto.Id = preference.Id;
            return dto;
        }

        public bool DeletePreference(int preferenceId)
        {
            var preference = _preferences.FirstOrDefault(p => p.Id == preferenceId);
            if (preference == null) return false;
            return _preferences.Remove(preference);
        }

        // Statistics
        public object GetLoyaltyStatistics()
        {
            return new
            {
                TotalCustomers = _customers.Count,
                TotalPointsEarned = _transactions.Where(t => t.TransactionType == "Earn").Sum(t => t.Points),
                TotalPointsRedeemed = _transactions.Where(t => t.TransactionType == "Redeem").Sum(t => t.Points),
                AveragePointsPerCustomer = _customers.Average(c => c.LoyaltyPoints),
                ByMembershipLevel = _customers.GroupBy(c => c.MembershipLevel).Select(g => new
                {
                    Level = g.Key,
                    Count = g.Count(),
                    AveragePoints = g.Average(c => c.LoyaltyPoints)
                }),
                TopCustomers = _customers.OrderByDescending(c => c.LoyaltyPoints).Take(5).Select(c => new
                {
                    c.CustomerNumber,
                    c.FirstName,
                    c.LastName,
                    c.LoyaltyPoints,
                    c.MembershipLevel
                })
            };
        }

        // Helper methods
        private CustomerDto MapToDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                CustomerNumber = customer.CustomerNumber,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Country = customer.Country,
                City = customer.City,
                Address = customer.Address,
                DateOfBirth = customer.DateOfBirth,
                RegistrationDate = customer.RegistrationDate,
                LastActivityDate = customer.LastActivityDate,
                Status = customer.Status,
                TotalStays = customer.TotalStays,
                TotalNights = customer.TotalNights,
                TotalSpent = customer.TotalSpent,
                LoyaltyPoints = customer.LoyaltyPoints,
                MembershipLevel = customer.MembershipLevel,
                PreferredLanguage = customer.PreferredLanguage,
                Notes = customer.Notes,
                FullName = $"{customer.FirstName} {customer.LastName}"
            };
        }

        private LoyaltyTransactionDto MapToTransactionDto(LoyaltyTransaction transaction)
        {
            return new LoyaltyTransactionDto
            {
                Id = transaction.Id,
                TransactionType = transaction.TransactionType,
                Points = transaction.Points,
                PointsBefore = transaction.PointsBefore,
                PointsAfter = transaction.PointsAfter,
                Source = transaction.Source,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate,
                ExpiryDate = transaction.ExpiryDate
            };
        }

        public List<MembershipLevel> GetAllMembershipLevels()
        {
            return _membershipLevels;
        }
    }
}