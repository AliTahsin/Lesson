using ChannelManagement.API.Models;

namespace ChannelManagement.API.Repositories
{
    public class ChannelRepository : Repository<Channel>, IChannelRepository
    {
        public ChannelRepository(List<Channel> context) : base(context) { }

        public async Task<Channel> GetByCodeAsync(string code)
        {
            return await SingleOrDefaultAsync(c => c.Code == code);
        }

        public async Task<IEnumerable<Channel>> GetActiveChannelsAsync()
        {
            return await FindAsync(c => c.IsActive);
        }

        public async Task<IEnumerable<Channel>> GetByTypeAsync(string type)
        {
            return await FindAsync(c => c.Type == type);
        }
    }

    public class ChannelConnectionRepository : Repository<ChannelConnection>, IChannelConnectionRepository
    {
        public ChannelConnectionRepository(List<ChannelConnection> context) : base(context) { }

        public async Task<IEnumerable<ChannelConnection>> GetByHotelAsync(int hotelId)
        {
            return await FindAsync(cc => cc.HotelId == hotelId);
        }

        public async Task<ChannelConnection> GetByChannelAndHotelAsync(int channelId, int hotelId)
        {
            return await SingleOrDefaultAsync(cc => cc.ChannelId == channelId && cc.HotelId == hotelId);
        }

        public async Task<IEnumerable<ChannelConnection>> GetActiveConnectionsAsync()
        {
            return await FindAsync(cc => cc.ConnectionStatus == "Active" && cc.AutoSync);
        }
    }

    public class ChannelBookingRepository : Repository<ChannelBooking>, IChannelBookingRepository
    {
        public ChannelBookingRepository(List<ChannelBooking> context) : base(context) { }

        public async Task<IEnumerable<ChannelBooking>> GetByHotelAsync(int hotelId)
        {
            return await FindAsync(cb => cb.HotelId == hotelId);
        }

        public async Task<IEnumerable<ChannelBooking>> GetByChannelAsync(int channelId)
        {
            return await FindAsync(cb => cb.ChannelId == channelId);
        }

        public async Task<IEnumerable<ChannelBooking>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await FindAsync(cb => cb.BookingDate >= startDate && cb.BookingDate <= endDate);
        }

        public async Task<decimal> GetTotalRevenueByChannelAsync(int channelId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Where(cb => cb.ChannelId == channelId && cb.Status == "Completed");
            if (startDate.HasValue)
                query = query.Where(cb => cb.BookingDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(cb => cb.BookingDate <= endDate.Value);
            
            return await Task.FromResult(query.Sum(cb => cb.NetRevenue));
        }
    }

    public class SyncLogRepository : Repository<SyncLog>, ISyncLogRepository
    {
        public SyncLogRepository(List<SyncLog> context) : base(context) { }

        public async Task<IEnumerable<SyncLog>> GetByChannelAsync(int channelId, int? hotelId = null)
        {
            if (hotelId.HasValue)
                return await FindAsync(sl => sl.ChannelId == channelId && sl.HotelId == hotelId.Value);
            return await FindAsync(sl => sl.ChannelId == channelId);
        }

        public async Task<SyncLog> GetLastSyncLogAsync(int channelId, int hotelId, string syncType)
        {
            return await SingleOrDefaultAsync(sl => 
                sl.ChannelId == channelId && 
                sl.HotelId == hotelId && 
                sl.SyncType == syncType);
        }
    }
}