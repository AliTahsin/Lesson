namespace Reporting.API.DTOs
{
    public class ReportRequestDto
    {
        public int HotelId { get; set; }
        public string ReportType { get; set; } // Revenue, Occupancy, Reservation, Customer, Channel
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Format { get; set; } // Excel, PDF
    }

    public class ScheduledReportDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ReportId { get; set; }
        public string Frequency { get; set; }
        public string DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        public string TimeOfDay { get; set; }
        public string RecipientEmails { get; set; }
        public string Format { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastRunAt { get; set; }
        public DateTime NextRunAt { get; set; }
        public string LastRunStatus { get; set; }
    }

    public class CreateScheduledReportDto
    {
        public string Name { get; set; }
        public int ReportId { get; set; }
        public string Frequency { get; set; }
        public string DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        public string TimeOfDay { get; set; }
        public string RecipientEmails { get; set; }
        public string Format { get; set; }
    }
}