namespace RoomManagement.API.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; } // Standard, Deluxe, Suite, Presidential
        public string Code { get; set; } // STD, DLX, STE, PRS
        public string Description { get; set; }
        public int DefaultCapacity { get; set; }
        public decimal DefaultPrice { get; set; }
        public string Icon { get; set; }
        public List<string> StandardAmenities { get; set; }
        public string ImageUrl { get; set; }
    }
}