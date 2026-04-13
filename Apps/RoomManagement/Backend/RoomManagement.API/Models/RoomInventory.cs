namespace RoomManagement.API.Models
{
    public class RoomInventory
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public int AvailableCount { get; set; }
        public int BookedCount { get; set; }
        public int MaintenanceCount { get; set; }
    }
}