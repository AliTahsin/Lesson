namespace RoomManagement.API.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsAvailable { get; set; }
        public string Status { get; set; }
    }
}
