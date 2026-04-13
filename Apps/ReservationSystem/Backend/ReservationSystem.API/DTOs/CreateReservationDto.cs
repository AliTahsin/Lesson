using System;

namespace ReservationSystem.API.DTOs
{
    public class CreateReservationDto
    {
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int GuestCount { get; set; }
        public int ChildCount { get; set; }
        public string SpecialRequests { get; set; }
        public string PaymentMethod { get; set; }
        public string Source { get; set; }
    }
}
