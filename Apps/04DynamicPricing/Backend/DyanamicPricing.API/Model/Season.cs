namespace DynamicPricing.API.Models
{
    public class Season
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; } // Low, Mid, High, Peak
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public decimal Multiplier { get; set; }
        public string Color { get; set; }
    }
}