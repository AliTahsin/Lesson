namespace CRM_Loyalty.API.Models
{
    public class MembershipLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinPoints { get; set; }
        public int MaxPoints { get; set; }
        public decimal PointsMultiplier { get; set; }
        public decimal DiscountRate { get; set; }
        public int FreeUpgradePerYear { get; set; }
        public int LateCheckoutHours { get; set; }
        public int EarlyCheckinHours { get; set; }
        public bool FreeBreakfast { get; set; }
        public bool AirportTransfer { get; set; }
        public bool LoungeAccess { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
    }
}