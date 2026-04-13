namespace ChannelManagement.API.DTOs
{
    public class ChannelConnectionDto
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string ConnectionStatus { get; set; }
        public DateTime ConnectedAt { get; set; }
        public DateTime? LastSyncAt { get; set; }
        public string LastSyncStatus { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public bool AutoSync { get; set; }
    }

    public class ChannelConnectionDetailDto : ChannelConnectionDto
    {
        public string Configuration { get; set; }
        public string LastSyncError { get; set; }
    }

    public class ConnectChannelDto
    {
        public int ChannelId { get; set; }
        public int HotelId { get; set; }
        public Dictionary<string, string> Configuration { get; set; }
        public bool AutoSync { get; set; }
    }
}