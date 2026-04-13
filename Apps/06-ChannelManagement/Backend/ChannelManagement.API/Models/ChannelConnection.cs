namespace ChannelManagement.API.Models
{
    public class ChannelConnection
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public int HotelId { get; set; }
        public string ConnectionStatus { get; set; } // Active, Inactive, Error
        public string Configuration { get; set; } // JSON string
        public DateTime ConnectedAt { get; set; }
        public DateTime? LastSyncAt { get; set; }
        public string LastSyncStatus { get; set; }
        public string LastSyncError { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public bool AutoSync { get; set; }
    }
}