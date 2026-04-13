namespace Reporting.API.DTOs
{
    public class KPIDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int HotelId { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal PreviousValue { get; set; }
        public decimal TargetValue { get; set; }
        public decimal ChangePercent { get; set; }
        public string Trend { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class CreateKPIDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int HotelId { get; set; }
        public decimal TargetValue { get; set; }
    }

    public class UpdateKPIDto
    {
        public decimal CurrentValue { get; set; }
        public decimal PreviousValue { get; set; }
        public decimal ChangePercent { get; set; }
        public string Trend { get; set; }
        public string Status { get; set; }
    }
}