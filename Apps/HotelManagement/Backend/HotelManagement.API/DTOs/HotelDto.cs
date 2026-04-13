namespace HotelManagement.API.DTOs
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string ChainName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int StarRating { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<string> Amenities { get; set; }
    }

    public class HotelDetailDto : HotelDto
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public int TotalRooms { get; set; }
        public DateTime OpeningDate { get; set; }
        public List<string> Images { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class CreateHotelDto
    {
        public string Name { get; set; }
        public int BrandId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public int StarRating { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public List<string> Amenities { get; set; }
    }
}