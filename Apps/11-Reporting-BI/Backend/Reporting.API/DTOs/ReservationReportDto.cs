namespace Reporting.API.DTOs
{
    public class ReservationReportDto
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalReservations { get; set; }
        public int ConfirmedReservations { get; set; }
        public int CancelledReservations { get; set; }
        public int NoShowReservations { get; set; }
        public decimal CancellationRate { get; set; }
        public decimal NoShowRate { get; set; }
        public List<DailyReservationDto> ReservationData { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class DailyReservationDto
    {
        public DateTime Date { get; set; }
        public int TotalReservations { get; set; }
        public int ConfirmedReservations { get; set; }
        public int CancelledReservations { get; set; }
        public int NoShowReservations { get; set; }
        public int AverageLeadDays { get; set; }
    }
}