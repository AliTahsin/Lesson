using ChannelManagement.API.Models;
using ChannelManagement.API.DTOs;

namespace ChannelManagement.API.Data
{
    public static class MockData
    {
        public static List<Channel> GetChannels()
        {
            return new List<Channel>
            {
                new Channel 
                { 
                    Id = 1, Name = "Booking.com", Code = "BOOKING", Type = "OTA",
                    ApiEndpoint = "https://api.booking.com/v1", ApiKey = "bk_12345", ApiSecret = "secret1",
                    Commission = 0.15m, Markup = 0.10m, IsActive = true,
                    SyncIntervalMinutes = 30, LastSyncDate = DateTime.Now.AddHours(-2),
                    LogoUrl = "booking_logo.png", Description = "World's leading OTA",
                    SupportedCurrencies = new List<string> { "USD", "EUR", "GBP", "TRY" },
                    SupportedLanguages = new List<string> { "en", "tr", "de", "fr" }
                },
                new Channel 
                { 
                    Id = 2, Name = "Expedia", Code = "EXPEDIA", Type = "OTA",
                    ApiEndpoint = "https://api.expedia.com/v1", ApiKey = "ex_67890", ApiSecret = "secret2",
                    Commission = 0.18m, Markup = 0.12m, IsActive = true,
                    SyncIntervalMinutes = 30, LastSyncDate = DateTime.Now.AddHours(-3),
                    LogoUrl = "expedia_logo.png", Description = "Global travel platform",
                    SupportedCurrencies = new List<string> { "USD", "EUR", "GBP" },
                    SupportedLanguages = new List<string> { "en", "es", "fr", "de" }
                },
                new Channel 
                { 
                    Id = 3, Name = "Agoda", Code = "AGODA", Type = "OTA",
                    ApiEndpoint = "https://api.agoda.com/v1", ApiKey = "ag_11111", ApiSecret = "secret3",
                    Commission = 0.12m, Markup = 0.08m, IsActive = true,
                    SyncIntervalMinutes = 30, LastSyncDate = DateTime.Now.AddHours(-1),
                    LogoUrl = "agoda_logo.png", Description = "Asia's leading OTA",
                    SupportedCurrencies = new List<string> { "USD", "EUR", "SGD", "THB" },
                    SupportedLanguages = new List<string> { "en", "zh", "ja", "ko" }
                },
                new Channel 
                { 
                    Id = 4, Name = "Amadeus", Code = "AMADEUS", Type = "GDS",
                    ApiEndpoint = "https://api.amadeus.com/v1", ApiKey = "am_22222", ApiSecret = "secret4",
                    Commission = 0.10m, Markup = 0.05m, IsActive = true,
                    SyncIntervalMinutes = 15, LastSyncDate = DateTime.Now.AddMinutes(-45),
                    LogoUrl = "amadeus_logo.png", Description = "Global GDS",
                    SupportedCurrencies = new List<string> { "USD", "EUR", "GBP" },
                    SupportedLanguages = new List<string> { "en", "fr", "de", "es" }
                },
                new Channel 
                { 
                    Id = 5, Name = "Sabre", Code = "SABRE", Type = "GDS",
                    ApiEndpoint = "https://api.sabre.com/v1", ApiKey = "sb_33333", ApiSecret = "secret5",
                    Commission = 0.10m, Markup = 0.05m, IsActive = false,
                    SyncIntervalMinutes = 15, LastSyncDate = DateTime.Now.AddDays(-5),
                    LogoUrl = "sabre_logo.png", Description = "Travel technology company",
                    SupportedCurrencies = new List<string> { "USD", "EUR" },
                    SupportedLanguages = new List<string> { "en" }
                },
                new Channel 
                { 
                    Id = 6, Name = "Google Hotels", Code = "GOOGLE", Type = "Meta",
                    ApiEndpoint = "https://api.google.com/hotels", ApiKey = "go_44444", ApiSecret = "secret6",
                    Commission = 0.08m, Markup = 0.03m, IsActive = true,
                    SyncIntervalMinutes = 60, LastSyncDate = DateTime.Now.AddHours(-4),
                    LogoUrl = "google_logo.png", Description = "Metasearch platform",
                    SupportedCurrencies = new List<string> { "USD", "EUR", "GBP", "TRY" },
                    SupportedLanguages = new List<string> { "en", "tr", "de", "fr", "es" }
                },
                new Channel 
                { 
                    Id = 7, Name = "Direct Booking", Code = "DIRECT", Type = "Direct",
                    ApiEndpoint = null, ApiKey = null, ApiSecret = null,
                    Commission = 0m, Markup = 0m, IsActive = true,
                    SyncIntervalMinutes = 0, LastSyncDate = DateTime.Now,
                    LogoUrl = "direct_logo.png", Description = "Direct website bookings",
                    SupportedCurrencies = new List<string> { "USD", "EUR", "TRY" },
                    SupportedLanguages = new List<string> { "en", "tr" }
                }
            };
        }

        public static List<ChannelConnection> GetChannelConnections()
        {
            return new List<ChannelConnection>
            {
                new ChannelConnection 
                { 
                    Id = 1, ChannelId = 1, HotelId = 1, ConnectionStatus = "Active",
                    Configuration = "{\"propertyId\":\"MAR_IST_001\",\"currency\":\"EUR\"}",
                    ConnectedAt = new DateTime(2023, 1, 15), LastSyncAt = DateTime.Now.AddMinutes(-15),
                    LastSyncStatus = "Success", TotalBookings = 245, TotalRevenue = 36750m, AutoSync = true
                },
                new ChannelConnection 
                { 
                    Id = 2, ChannelId = 1, HotelId = 2, ConnectionStatus = "Active",
                    Configuration = "{\"propertyId\":\"HIL_IZM_001\",\"currency\":\"EUR\"}",
                    ConnectedAt = new DateTime(2023, 2, 10), LastSyncAt = DateTime.Now.AddMinutes(-20),
                    LastSyncStatus = "Success", TotalBookings = 189, TotalRevenue = 22680m, AutoSync = true
                },
                new ChannelConnection 
                { 
                    Id = 3, ChannelId = 2, HotelId = 1, ConnectionStatus = "Active",
                    Configuration = "{\"propertyId\":\"EXP_MAR_001\",\"currency\":\"EUR\"}",
                    ConnectedAt = new DateTime(2023, 1, 20), LastSyncAt = DateTime.Now.AddMinutes(-25),
                    LastSyncStatus = "Success", TotalBookings = 178, TotalRevenue = 26700m, AutoSync = true
                },
                new ChannelConnection 
                { 
                    Id = 4, ChannelId = 3, HotelId = 3, ConnectionStatus = "Active",
                    Configuration = "{\"propertyId\":\"AGO_SOF_001\",\"currency\":\"EUR\"}",
                    ConnectedAt = new DateTime(2023, 3, 5), LastSyncAt = DateTime.Now.AddMinutes(-10),
                    LastSyncStatus = "Success", TotalBookings = 92, TotalRevenue = 27600m, AutoSync = true
                },
                new ChannelConnection 
                { 
                    Id = 5, ChannelId = 4, HotelId = 1, ConnectionStatus = "Error",
                    Configuration = "{\"agencyId\":\"TRV_001\"}",
                    ConnectedAt = new DateTime(2023, 4, 1), LastSyncAt = DateTime.Now.AddDays(-2),
                    LastSyncStatus = "Failed", LastSyncError = "API connection timeout",
                    TotalBookings = 0, TotalRevenue = 0m, AutoSync = true
                },
                new ChannelConnection 
                { 
                    Id = 6, ChannelId = 6, HotelId = 1, ConnectionStatus = "Active",
                    Configuration = "{\"hotelId\":\"G_MAR_001\"}",
                    ConnectedAt = new DateTime(2023, 5, 10), LastSyncAt = DateTime.Now.AddHours(-2),
                    LastSyncStatus = "Success", TotalBookings = 67, TotalRevenue = 10050m, AutoSync = true
                },
                new ChannelConnection 
                { 
                    Id = 7, ChannelId = 7, HotelId = 1, ConnectionStatus = "Active",
                    Configuration = "{\"website\":\"marriott.com\"}",
                    ConnectedAt = new DateTime(2023, 1, 1), LastSyncAt = DateTime.Now,
                    LastSyncStatus = "Success", TotalBookings = 520, TotalRevenue = 78000m, AutoSync = false
                }
            };
        }

        public static List<ChannelBooking> GetChannelBookings()
        {
            var bookings = new List<ChannelBooking>();
            var random = new Random();
            var channels = GetChannels();
            var hotels = new[] { 1, 2, 3 };
            var rooms = new[] { 101, 102, 103, 201, 202, 301 };

            for (int i = 0; i < 200; i++)
            {
                var channel = channels[random.Next(channels.Count)];
                var hotelId = hotels[random.Next(hotels.Length)];
                var checkIn = DateTime.Today.AddDays(random.Next(-30, 60));
                var checkOut = checkIn.AddDays(random.Next(1, 7));
                var price = random.Next(100, 500);
                var commission = price * channel.Commission;
                var netRevenue = price - commission;

                bookings.Add(new ChannelBooking
                {
                    Id = i + 1,
                    ChannelId = channel.Id,
                    HotelId = hotelId,
                    ChannelBookingId = $"{channel.Code}-{DateTime.Now.Year}-{i + 1000:D4}",
                    GuestName = $"Guest {i + 1}",
                    GuestEmail = $"guest{i + 1}@email.com",
                    RoomId = rooms[random.Next(rooms.Length)],
                    CheckInDate = checkIn,
                    CheckOutDate = checkOut,
                    GuestCount = random.Next(1, 5),
                    TotalPrice = price * (checkOut - checkIn).Days,
                    Commission = commission * (checkOut - checkIn).Days,
                    NetRevenue = netRevenue * (checkOut - checkIn).Days,
                    Currency = "EUR",
                    Status = random.Next(0, 10) > 8 ? "Cancelled" : (checkIn < DateTime.Today ? "Completed" : "Confirmed"),
                    BookingDate = DateTime.Today.AddDays(-random.Next(0, 60)),
                    SyncDate = DateTime.Now.AddMinutes(-random.Next(0, 120)),
                    Notes = ""
                });
            }

            return bookings;
        }

        public static List<SyncLog> GetSyncLogs()
        {
            var logs = new List<SyncLog>();
            var channels = GetChannels();
            var hotels = new[] { 1, 2, 3 };
            var syncTypes = new[] { "Availability", "Price", "Booking" };
            var random = new Random();

            for (int i = 0; i < 500; i++)
            {
                var channel = channels[random.Next(channels.Count)];
                var hotelId = hotels[random.Next(hotels.Length)];
                var syncType = syncTypes[random.Next(syncTypes.Length)];
                var startTime = DateTime.Now.AddMinutes(-random.Next(0, 720));
                var duration = random.Next(1, 30);
                var isSuccess = random.Next(0, 10) > 2;

                logs.Add(new SyncLog
                {
                    Id = i + 1,
                    ChannelId = channel.Id,
                    HotelId = hotelId,
                    SyncType = syncType,
                    StartTime = startTime,
                    EndTime = startTime.AddSeconds(duration),
                    Status = isSuccess ? "Success" : "Failed",
                    RecordsProcessed = random.Next(10, 200),
                    RecordsSuccess = isSuccess ? random.Next(10, 200) : random.Next(0, 50),
                    RecordsFailed = isSuccess ? 0 : random.Next(1, 50),
                    ErrorMessage = isSuccess ? null : "Sync failed due to network error",
                    Details = isSuccess ? $"Successfully synced {syncType.ToLower()} data" : null
                });
            }

            return logs.OrderByDescending(l => l.StartTime).ToList();
        }
    }
}