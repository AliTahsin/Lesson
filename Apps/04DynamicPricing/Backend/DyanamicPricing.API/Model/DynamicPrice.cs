namespace DynamicPricing.API.Models
{
    public class DynamicPrice
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public decimal BasePrice { get; set; }
        public decimal CalculatedPrice { get; set; }
        public decimal OccupancyMultiplier { get; set; }
        public decimal DemandMultiplier { get; set; }
        public decimal SeasonMultiplier { get; set; }
        public decimal CompetitorMultiplier { get; set; }
        public decimal SpecialEventMultiplier { get; set; }
        public decimal FinalPrice { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}