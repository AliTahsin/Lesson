namespace HotelManagement.API.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public int StarRating { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public int TotalRooms { get; set; }
        public DateTime OpeningDate { get; set; }
        public string Status { get; set; } // Active, Maintenance, Closed
        public List<string> Amenities { get; set; }
        public List<string> Images { get; set; }
        public GeoLocation Location { get; set; }
    }

    public class GeoLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}