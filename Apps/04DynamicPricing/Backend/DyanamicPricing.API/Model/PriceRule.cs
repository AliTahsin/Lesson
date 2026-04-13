namespace DynamicPricing.API.Models
{
    public class PriceRule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RuleType { get; set; } // Occupancy, Demand, Season, Competitor, Event
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal Multiplier { get; set; }
        public decimal FixedAdjustment { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}