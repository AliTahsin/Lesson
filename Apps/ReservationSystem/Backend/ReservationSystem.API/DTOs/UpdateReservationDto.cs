using System;

namespace ReservationSystem.API.DTOs
{
    public class UpdateReservationDto
    {
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int? GuestCount { get; set; }
        public string SpecialRequests { get; set; }
    }
}
