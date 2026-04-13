namespace HotelManagement.API.Models
{
    public class Chain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Headquarters { get; set; }
        public string Founder { get; set; }
        public int FoundedYear { get; set; }
        public int TotalHotels { get; set; }
        public string Website { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
    }
}