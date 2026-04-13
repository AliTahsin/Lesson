namespace ReservationSystem.API.DTOs
{
    public class CheckInDto
    {
        public string ReservationNumber { get; set; }
        public string GuestEmail { get; set; }
        public string PassportNumber { get; set; }
        public List<string> AdditionalGuestNames { get; set; }
    }

    public class CheckOutDto
    {
        public string ReservationNumber { get; set; }
        public string GuestEmail { get; set; }
        public decimal? ExtraCharges { get; set; }
        public string ExtraChargesDescription { get; set; }
    }

    public class CheckInOutResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ReservationResponseDto Reservation { get; set; }
        public string DigitalKey { get; set; }
        public decimal? TotalExtraCharges { get; set; }
    }
}