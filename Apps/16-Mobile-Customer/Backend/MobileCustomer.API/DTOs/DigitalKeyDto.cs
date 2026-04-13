namespace MobileCustomer.API.DTOs
{
    public class DigitalKeyDto
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string KeyCode { get; set; }
        public string QrCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsActive { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? UsedAt { get; set; }
    }

    public class ValidateKeyDto
    {
        public string KeyCode { get; set; }
        public int RoomId { get; set; }
    }
}