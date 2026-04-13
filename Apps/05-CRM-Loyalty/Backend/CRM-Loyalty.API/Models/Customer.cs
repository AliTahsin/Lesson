namespace CRM_Loyalty.API.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string Status { get; set; } // Active, Inactive, Blocked
        public int TotalStays { get; set; }
        public int TotalNights { get; set; }
        public decimal TotalSpent { get; set; }
        public int LoyaltyPoints { get; set; }
        public string MembershipLevel { get; set; } // Bronze, Silver, Gold, Platinum, Diamond
        public string PreferredLanguage { get; set; }
        public string Notes { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}