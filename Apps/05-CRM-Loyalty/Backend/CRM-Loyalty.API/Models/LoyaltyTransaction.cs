namespace CRM_Loyalty.API.Models
{
    public class LoyaltyTransaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string TransactionType { get; set; } // Earn, Redeem, Bonus, Expire
        public int Points { get; set; }
        public int PointsBefore { get; set; }
        public int PointsAfter { get; set; }
        public string Source { get; set; } // Reservation, Review, Birthday, Promotion
        public string SourceId { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}