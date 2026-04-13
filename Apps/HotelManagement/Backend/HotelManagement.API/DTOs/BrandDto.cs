namespace HotelManagement.API.DTOs
{
    public class BrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChainName { get; set; }
        public string Segment { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public int MinStarRating { get; set; }
        public int MaxStarRating { get; set; }
    }
}