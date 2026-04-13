namespace Reporting.API.DTOs
{
    public class RevenueReportDto
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalRoomRevenue { get; set; }
        public decimal TotalFBRevenue { get; set; }
        public decimal TotalOtherRevenue { get; set; }
        public decimal AverageDailyRate { get; set; } // ADR
        public decimal RevPAR { get; set; } // Revenue Per Available Room
        public List<DailyRevenueDto> DailyData { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class DailyRevenueDto
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public decimal RoomRevenue { get; set; }
        public decimal FBRevenue { get; set; }
        public decimal OtherRevenue { get; set; }
    }
}