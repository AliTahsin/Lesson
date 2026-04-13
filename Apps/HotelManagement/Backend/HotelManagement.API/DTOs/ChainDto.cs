namespace HotelManagement.API.DTOs
{
    public class ChainDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Headquarters { get; set; }
        public int FoundedYear { get; set; }
        public int TotalHotels { get; set; }
        public string Website { get; set; }
        public string LogoUrl { get; set; }
    }

    public class ChainDetailDto : ChainDto
    {
        public string Founder { get; set; }
        public string Description { get; set; }
        public List<BrandDto> Brands { get; set; }
    }
}