namespace ReservationSystem.API.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string ReservationNumber { get; set; }
        public int GuestId { get; set; }
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime ActualCheckIn { get; set; }
        public DateTime ActualCheckOut { get; set; }
        public int GuestCount { get; set; }
        public int ChildCount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PaidAmount { get; set; }
        public string Status { get; set; } // Pending, Confirmed, CheckedIn, CheckedOut, Cancelled, NoShow
        public string PaymentStatus { get; set; } // Pending, Partial, Paid, Refunded
        public string PaymentMethod { get; set; }
        public string SpecialRequests { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string CancellationReason { get; set; }
        public string Source { get; set; } // Web, Mobile, CallCenter, OTA
    }
}