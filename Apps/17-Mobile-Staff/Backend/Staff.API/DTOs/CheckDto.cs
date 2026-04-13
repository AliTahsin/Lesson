namespace Staff.API.DTOs
{
    public class CheckDto
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string ProcessedByStaffName { get; set; }
        public string Type { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string Notes { get; set; }
        public string DigitalKey { get; set; }
    }

    public class CheckInDto
    {
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int HotelId { get; set; }
        public string Notes { get; set; }
    }

    public class CheckOutDto
    {
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int HotelId { get; set; }
        public string Notes { get; set; }
    }
}