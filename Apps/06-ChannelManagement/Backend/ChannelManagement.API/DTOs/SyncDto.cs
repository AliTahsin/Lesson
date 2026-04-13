namespace ChannelManagement.API.DTOs
{
    public class SyncRequest
    {
        public int ChannelId { get; set; }
        public int HotelId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class SyncAvailabilityRequest : SyncRequest
    {
        public List<int> RoomIds { get; set; }
    }

    public class SyncPriceRequest : SyncRequest
    {
        public List<int> RoomIds { get; set; }
        public decimal PriceMultiplier { get; set; } = 1.0m;
    }

    public class SyncResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int RecordsProcessed { get; set; }
        public int RecordsSuccess { get; set; }
        public int RecordsFailed { get; set; }
        public List<string> Errors { get; set; }
        public DateTime SyncTime { get; set; }
    }

    public class SyncLogDto
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public int HotelId { get; set; }
        public string SyncType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
        public int RecordsProcessed { get; set; }
        public int RecordsSuccess { get; set; }
        public int RecordsFailed { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ChannelBookingDto
    {
        public int Id { get; set; }
        public string ChannelBookingId { get; set; }
        public string ChannelName { get; set; }
        public string HotelName { get; set; }
        public string RoomNumber { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NightCount { get; set; }
        public int GuestCount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Commission { get; set; }
        public decimal NetRevenue { get; set; }
        public string Status { get; set; }
        public DateTime BookingDate { get; set; }
    }

    public class DashboardStatsDto
    {
        public int TotalChannels { get; set; }
        public int ActiveChannels { get; set; }
        public int TotalConnections { get; set; }
        public int ActiveConnections { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal NetRevenue { get; set; }
        public int TotalBookings { get; set; }
        public List<ChannelStatsDto> ChannelStats { get; set; }
        public List<DailyBookingDto> RecentBookings { get; set; }
    }

    public class ChannelStatsDto
    {
        public string ChannelName { get; set; }
        public int BookingCount { get; set; }
        public decimal Revenue { get; set; }
        public decimal Commission { get; set; }
        public decimal NetRevenue { get; set; }
    }

    public class DailyBookingDto
    {
        public DateTime Date { get; set; }
        public int BookingCount { get; set; }
        public decimal Revenue { get; set; }
    }
}