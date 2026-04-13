namespace Reporting.API.DTOs
{
    public class CustomerReportDto
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalCustomers { get; set; }
        public int NewCustomers { get; set; }
        public int RepeatCustomers { get; set; }
        public decimal CustomerSatisfactionScore { get; set; }
        public List<TopCustomerDto> TopCustomers { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class TopCustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int TotalStays { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageSpentPerStay { get; set; }
    }

    public class CustomerSegmentDto
    {
        public string Segment { get; set; } // New, Repeat, VIP, AtRisk
        public int Count { get; set; }
        public decimal Percentage { get; set; }
        public decimal TotalSpent { get; set; }
    }
}