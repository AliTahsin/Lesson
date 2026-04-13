namespace ReservationSystem.API.DTOs
{
    public class ReservationResponseDto
    {
        public int Id { get; set; }
        public string ReservationNumber { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string HotelName { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NightCount { get; set; }
        public int GuestCount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string SpecialRequests { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool CanCancel { get; set; }
        public bool CanModify { get; set; }
    }
}