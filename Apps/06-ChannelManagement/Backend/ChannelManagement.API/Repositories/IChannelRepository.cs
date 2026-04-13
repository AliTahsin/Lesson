using ChannelManagement.API.Models;

namespace ChannelManagement.API.Repositories
{
    public interface IChannelRepository : IRepository<Channel>
    {
        Task<Channel> GetByCodeAsync(string code);
        Task<IEnumerable<Channel>> GetActiveChannelsAsync();
        Task<IEnumerable<Channel>> GetByTypeAsync(string type);
    }

    public interface IChannelConnectionRepository : IRepository<ChannelConnection>
    {
        Task<IEnumerable<ChannelConnection>> GetByHotelAsync(int hotelId);
        Task<ChannelConnection> GetByChannelAndHotelAsync(int channelId, int hotelId);
        Task<IEnumerable<ChannelConnection>> GetActiveConnectionsAsync();
    }

    public interface IChannelBookingRepository : IRepository<ChannelBooking>
    {
        Task<IEnumerable<ChannelBooking>> GetByHotelAsync(int hotelId);
        Task<IEnumerable<ChannelBooking>> GetByChannelAsync(int channelId);
        Task<IEnumerable<ChannelBooking>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalRevenueByChannelAsync(int channelId, DateTime? startDate = null, DateTime? endDate = null);
    }

    public interface ISyncLogRepository : IRepository<SyncLog>
    {
        Task<IEnumerable<SyncLog>> GetByChannelAsync(int channelId, int? hotelId = null);
        Task<SyncLog> GetLastSyncLogAsync(int channelId, int hotelId, string syncType);
    }
}