namespace Reporting.API.DTOs
{
    public class OccupancyReportDto
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal AverageOccupancyRate { get; set; }
        public int TotalAvailableRooms { get; set; }
        public int TotalSoldRooms { get; set; }
        public List<DailyOccupancyDto> OccupancyData { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class DailyOccupancyDto
    {
        public DateTime Date { get; set; }
        public decimal OccupancyRate { get; set; }
        public int AvailableRooms { get; set; }
        public int SoldRooms { get; set; }
        public decimal AveragePrice { get; set; }
    }
}