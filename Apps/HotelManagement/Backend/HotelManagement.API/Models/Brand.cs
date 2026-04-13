namespace HotelManagement.API.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChainId { get; set; }
        public string Segment { get; set; } // Luxury, Upscale, Midscale, Economy
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public int MinStarRating { get; set; }
        public int MaxStarRating { get; set; }
    }
}