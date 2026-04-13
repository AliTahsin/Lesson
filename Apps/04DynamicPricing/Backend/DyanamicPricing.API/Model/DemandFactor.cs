namespace DynamicPricing.API.Models
{
    public class DemandFactor
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public DateTime Date { get; set; }
        public int DemandScore { get; set; } // 0-100
        public decimal ExpectedOccupancy { get; set; } // 0-100
        public int WebSearchCount { get; set; }
        public int BookingAttempts { get; set; }
        public List<string> Events { get; set; }
        public string Notes { get; set; }
    }
}