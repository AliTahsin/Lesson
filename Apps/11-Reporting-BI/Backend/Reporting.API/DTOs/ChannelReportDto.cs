namespace Reporting.API.DTOs
{
    public class ChannelReportDto
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal NetRevenue { get; set; }
        public List<ChannelPerformanceDto> ChannelData { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class ChannelPerformanceDto
    {
        public string ChannelName { get; set; }
        public int Bookings { get; set; }
        public decimal Revenue { get; set; }
        public decimal Commission { get; set; }
        public decimal NetRevenue { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal AverageBookingValue { get; set; }
    }
}