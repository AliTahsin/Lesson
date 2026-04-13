namespace RoomManagement.API.DTOs
{
    public class RoomInventoryDto
    {
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
    }
}
