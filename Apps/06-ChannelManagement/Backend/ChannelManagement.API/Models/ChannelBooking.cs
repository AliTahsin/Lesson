namespace ChannelManagement.API.Models
{
    public class ChannelBooking
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public int HotelId { get; set; }
        public string ChannelBookingId { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int GuestCount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Commission { get; set; }
        public decimal NetRevenue { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Cancelled, Completed
        public DateTime BookingDate { get; set; }
        public DateTime? SyncDate { get; set; }
        public string Notes { get; set; }
    }
}