namespace RoomManagement.API.DTOs
{
    public class RoomTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DefaultCapacity { get; set; }
        public decimal DefaultPrice { get; set; }
    }
}
