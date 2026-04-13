namespace RoomManagement.API.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomNumber { get; set; }
        public int Floor { get; set; }
        public string View { get; set; } // Sea, City, Mountain, Garden
        public int Capacity { get; set; }
        public int ExtraBedCapacity { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsClean { get; set; }
        public string Status { get; set; } // Available, Occupied, Maintenance, Cleaning
        public List<string> Amenities { get; set; }
        public string Description { get; set; }
    }
}