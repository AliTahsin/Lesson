using AutoMapper;
using ChannelManagement.API.Models;
using ChannelManagement.API.DTOs;
using ChannelManagement.API.Repositories;
using ChannelManagement.API.Data;

namespace ChannelManagement.API.Services
{
    public class ChannelSyncService
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IChannelConnectionRepository _connectionRepository;
        private readonly IChannelBookingRepository _bookingRepository;
        private readonly ISyncLogRepository _syncLogRepository;
        private readonly IMapper _mapper;

        public ChannelSyncService(
            IChannelRepository channelRepository,
            IChannelConnectionRepository connectionRepository,
            IChannelBookingRepository bookingRepository,
            ISyncLogRepository syncLogRepository,
            IMapper mapper)
        {
            _channelRepository = channelRepository;
            _connectionRepository = connectionRepository;
            _bookingRepository = bookingRepository;
            _syncLogRepository = syncLogRepository;
            _mapper = mapper;
        }

        // Channel CRUD
        public async Task<IEnumerable<ChannelDto>> GetAllChannelsAsync()
        {
            var channels = await _channelRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ChannelDto>>(channels);
        }

        public async Task<ChannelDetailDto> GetChannelByIdAsync(int id)
        {
            var channel = await _channelRepository.GetByIdAsync(id);
            return channel != null ? _mapper.Map<ChannelDetailDto>(channel) : null;
        }

        public async Task<ChannelDto> CreateChannelAsync(CreateChannelDto dto)
        {
            var channel = _mapper.Map<Channel>(dto);
            channel.IsActive = true;
            channel.LastSyncDate = DateTime.Now;
            await _channelRepository.AddAsync(channel);
            return _mapper.Map<ChannelDto>(channel);
        }

        public async Task<bool> UpdateChannelAsync(int id, ChannelDetailDto dto)
        {
            var existing = await _channelRepository.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing);
            _channelRepository.Update(existing);
            return true;
        }

        // Connection Management
        public async Task<IEnumerable<ChannelConnectionDto>> GetHotelConnectionsAsync(int hotelId)
        {
            var connections = await _connectionRepository.GetByHotelAsync(hotelId);
            var result = _mapper.Map<IEnumerable<ChannelConnectionDto>>(connections);
            
            foreach (var conn in result)
            {
                var channel = await _channelRepository.GetByIdAsync(conn.ChannelId);
                conn.ChannelName = channel?.Name;
                conn.HotelName = GetHotelName(conn.HotelId);
            }
            
            return result;
        }

        public async Task<ChannelConnectionDto> ConnectChannelAsync(ConnectChannelDto dto)
        {
            var existing = await _connectionRepository.GetByChannelAndHotelAsync(dto.ChannelId, dto.HotelId);
            if (existing != null)
                throw new Exception("Connection already exists");

            var connection = new ChannelConnection
            {
                ChannelId = dto.ChannelId,
                HotelId = dto.HotelId,
                ConnectionStatus = "Active",
                Configuration = System.Text.Json.JsonSerializer.Serialize(dto.Configuration),
                ConnectedAt = DateTime.Now,
                AutoSync = dto.AutoSync,
                TotalBookings = 0,
                TotalRevenue = 0
            };

            await _connectionRepository.AddAsync(connection);
            return _mapper.Map<ChannelConnectionDto>(connection);
        }

        public async Task<bool> DisconnectChannelAsync(int connectionId)
        {
            var connection = await _connectionRepository.GetByIdAsync(connectionId);
            if (connection == null) return false;

            connection.ConnectionStatus = "Inactive";
            connection.AutoSync = false;
            _connectionRepository.Update(connection);
            return true;
        }

        // Sync Operations
        public async Task<SyncResponse> SyncAvailabilityAsync(SyncAvailabilityRequest request)
        {
            var log = new SyncLog
            {
                ChannelId = request.ChannelId,
                HotelId = request.HotelId,
                SyncType = "Availability",
                StartTime = DateTime.Now,
                Status = "Pending"
            };

            var response = new SyncResponse { SyncTime = DateTime.Now, Errors = new List<string>() };

            try
            {
                var channel = await _channelRepository.GetByIdAsync(request.ChannelId);
                if (channel == null)
                    throw new Exception("Channel not found");

                // Simulate API call
                await Task.Delay(1000);

                var roomsToSync = request.RoomIds ?? new List<int> { 1, 2, 3, 4, 5 };
                
                response.RecordsProcessed = roomsToSync.Count;
                response.RecordsSuccess = roomsToSync.Count;
                response.RecordsFailed = 0;
                response.Success = true;
                response.Message = $"Successfully synced availability for {roomsToSync.Count} rooms";

                log.Status = "Success";
                log.RecordsProcessed = response.RecordsProcessed;
                log.RecordsSuccess = response.RecordsSuccess;
                log.RecordsFailed = response.RecordsFailed;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Errors.Add(ex.Message);
                
                log.Status = "Failed";
                log.ErrorMessage = ex.Message;
                log.RecordsFailed = 1;
            }
            finally
            {
                log.EndTime = DateTime.Now;
                await _syncLogRepository.AddAsync(log);
            }

            return response;
        }

        public async Task<SyncResponse> SyncPricesAsync(SyncPriceRequest request)
        {
            var log = new SyncLog
            {
                ChannelId = request.ChannelId,
                HotelId = request.HotelId,
                SyncType = "Price",
                StartTime = DateTime.Now,
                Status = "Pending"
            };

            var response = new SyncResponse { SyncTime = DateTime.Now, Errors = new List<string>() };

            try
            {
                var channel = await _channelRepository.GetByIdAsync(request.ChannelId);
                if (channel == null)
                    throw new Exception("Channel not found");

                await Task.Delay(1000);

                var roomsToSync = request.RoomIds ?? new List<int> { 1, 2, 3, 4, 5 };
                
                response.RecordsProcessed = roomsToSync.Count;
                response.RecordsSuccess = roomsToSync.Count;
                response.RecordsFailed = 0;
                response.Success = true;
                response.Message = $"Successfully synced prices with {request.PriceMultiplier}x multiplier for {roomsToSync.Count} rooms";

                log.Status = "Success";
                log.RecordsProcessed = response.RecordsProcessed;
                log.RecordsSuccess = response.RecordsSuccess;
                log.RecordsFailed = response.RecordsFailed;

                // Update last sync date
                var connection = await _connectionRepository.GetByChannelAndHotelAsync(request.ChannelId, request.HotelId);
                if (connection != null)
                {
                    connection.LastSyncAt = DateTime.Now;
                    connection.LastSyncStatus = "Success";
                    _connectionRepository.Update(connection);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Errors.Add(ex.Message);
                
                log.Status = "Failed";
                log.ErrorMessage = ex.Message;
                log.RecordsFailed = 1;
            }
            finally
            {
                log.EndTime = DateTime.Now;
                await _syncLogRepository.AddAsync(log);
            }

            return response;
        }

        // Bookings
        public async Task<IEnumerable<ChannelBookingDto>> GetChannelBookingsAsync(int? channelId = null, int? hotelId = null)
        {
            IEnumerable<ChannelBooking> bookings;
            
            if (channelId.HasValue)
                bookings = await _bookingRepository.GetByChannelAsync(channelId.Value);
            else if (hotelId.HasValue)
                bookings = await _bookingRepository.GetByHotelAsync(hotelId.Value);
            else
                bookings = await _bookingRepository.GetAllAsync();

            var result = _mapper.Map<IEnumerable<ChannelBookingDto>>(bookings);
            
            foreach (var booking in result)
            {
                var channel = await _channelRepository.GetByIdAsync(booking.ChannelId);
                booking.ChannelName = channel?.Name;
                booking.HotelName = GetHotelName(booking.HotelId);
                booking.RoomNumber = $"ROOM-{booking.RoomId}";
                booking.NightCount = (booking.CheckOutDate - booking.CheckInDate).Days;
            }
            
            return result;
        }

        // Statistics
        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var channels = await _channelRepository.GetAllAsync();
            var activeChannels = channels.Where(c => c.IsActive);
            var connections = await _connectionRepository.GetAllAsync();
            var activeConnections = connections.Where(c => c.ConnectionStatus == "Active");
            var bookings = await _bookingRepository.GetAllAsync();
            var completedBookings = bookings.Where(b => b.Status == "Completed");

            var channelStats = new List<ChannelStatsDto>();
            foreach (var channel in channels)
            {
                var channelBookings = completedBookings.Where(b => b.ChannelId == channel.Id);
                channelStats.Add(new ChannelStatsDto
                {
                    ChannelName = channel.Name,
                    BookingCount = channelBookings.Count(),
                    Revenue = channelBookings.Sum(b => b.TotalPrice),
                    Commission = channelBookings.Sum(b => b.Commission),
                    NetRevenue = channelBookings.Sum(b => b.NetRevenue)
                });
            }

            var recentBookings = completedBookings
                .Where(b => b.BookingDate >= DateTime.Today.AddDays(-30))
                .GroupBy(b => b.BookingDate.Date)
                .Select(g => new DailyBookingDto
                {
                    Date = g.Key,
                    BookingCount = g.Count(),
                    Revenue = g.Sum(b => b.TotalPrice)
                })
                .OrderBy(d => d.Date)
                .ToList();

            return new DashboardStatsDto
            {
                TotalChannels = channels.Count(),
                ActiveChannels = activeChannels.Count(),
                TotalConnections = connections.Count(),
                ActiveConnections = activeConnections.Count(),
                TotalRevenue = completedBookings.Sum(b => b.TotalPrice),
                TotalCommission = completedBookings.Sum(b => b.Commission),
                NetRevenue = completedBookings.Sum(b => b.NetRevenue),
                TotalBookings = completedBookings.Count(),
                ChannelStats = channelStats,
                RecentBookings = recentBookings
            };
        }

        public async Task<IEnumerable<SyncLogDto>> GetSyncLogsAsync(int channelId, int? hotelId = null)
        {
            var logs = await _syncLogRepository.GetByChannelAsync(channelId, hotelId);
            return _mapper.Map<IEnumerable<SyncLogDto>>(logs.OrderByDescending(l => l.StartTime).Take(50));
        }

        private string GetHotelName(int hotelId)
        {
            return hotelId switch
            {
                1 => "Marriott Istanbul",
                2 => "Hilton Izmir",
                3 => "Sofitel Bodrum",
                _ => $"Hotel {hotelId}"
            };
        }
    }
}