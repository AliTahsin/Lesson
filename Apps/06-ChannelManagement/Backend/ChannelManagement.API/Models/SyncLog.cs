namespace ChannelManagement.API.Models
{
    public class SyncLog
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public int HotelId { get; set; }
        public string SyncType { get; set; } // Availability, Price, Booking
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; } // Success, Failed, Partial
        public int RecordsProcessed { get; set; }
        public int RecordsSuccess { get; set; }
        public int RecordsFailed { get; set; }
        public string ErrorMessage { get; set; }
        public string Details { get; set; }
    }
}