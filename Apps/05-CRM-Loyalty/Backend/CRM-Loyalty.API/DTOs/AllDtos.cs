namespace CRM_Loyalty.API.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string CustomerNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string Status { get; set; }
        public int TotalStays { get; set; }
        public int TotalNights { get; set; }
        public decimal TotalSpent { get; set; }
        public int LoyaltyPoints { get; set; }
        public string MembershipLevel { get; set; }
        public string PreferredLanguage { get; set; }
        public string Notes { get; set; }
    }

    public class LoyaltyTransactionDto
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public int Points { get; set; }
        public int PointsBefore { get; set; }
        public int PointsAfter { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class LoyaltyDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CurrentPoints { get; set; }
        public string CurrentLevel { get; set; }
        public int PointsToNextLevel { get; set; }
        public string NextLevel { get; set; }
        public LevelBenefitsDto LevelBenefits { get; set; }
        public int TotalStays { get; set; }
        public int TotalNights { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public class LevelBenefitsDto
    {
        public decimal DiscountRate { get; set; }
        public decimal PointsMultiplier { get; set; }
        public int FreeUpgradePerYear { get; set; }
        public int LateCheckoutHours { get; set; }
        public int EarlyCheckinHours { get; set; }
        public bool FreeBreakfast { get; set; }
        public bool AirportTransfer { get; set; }
        public bool LoungeAccess { get; set; }
    }

    public class AddPointsDto
    {
        public int CustomerId { get; set; }
        public int Points { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
    }

    public class RedeemPointsDto
    {
        public int CustomerId { get; set; }
        public int Points { get; set; }
        public string Description { get; set; }
    }

    public class CreateCustomerDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PreferredLanguage { get; set; }
    }
}
