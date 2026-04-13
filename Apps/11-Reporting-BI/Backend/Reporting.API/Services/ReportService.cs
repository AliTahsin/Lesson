using AutoMapper;
using Reporting.API.Models;
using Reporting.API.DTOs;
using Reporting.API.Repositories;

namespace Reporting.API.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IExcelExportService _excelExportService;
        private readonly IPdfExportService _pdfExportService;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            IReportRepository reportRepository,
            IExcelExportService excelExportService,
            IPdfExportService pdfExportService,
            IMapper mapper,
            ILogger<ReportService> logger)
        {
            _reportRepository = reportRepository;
            _excelExportService = excelExportService;
            _pdfExportService = pdfExportService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RevenueReportDto> GetRevenueReportAsync(int hotelId, DateTime startDate, DateTime endDate)
        {
            // Simulate data aggregation
            var revenueData = new List<DailyRevenueDto>();
            var random = new Random();
            
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                revenueData.Add(new DailyRevenueDto
                {
                    Date = date,
                    Revenue = random.Next(5000, 50000),
                    RoomRevenue = random.Next(3000, 35000),
                    FBRevenue = random.Next(1000, 10000),
                    OtherRevenue = random.Next(500, 5000)
                });
            }

            return new RevenueReportDto
            {
                HotelId = hotelId,
                HotelName = GetHotelName(hotelId),
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = revenueData.Sum(r => r.Revenue),
                TotalRoomRevenue = revenueData.Sum(r => r.RoomRevenue),
                TotalFBRevenue = revenueData.Sum(r => r.FBRevenue),
                TotalOtherRevenue = revenueData.Sum(r => r.OtherRevenue),
                AverageDailyRate = revenueData.Average(r => r.Revenue / 100), // Mock ADR
                RevPAR = revenueData.Average(r => r.Revenue / 200), // Mock RevPAR
                DailyData = revenueData,
                GeneratedAt = DateTime.Now
            };
        }

        public async Task<OccupancyReportDto> GetOccupancyReportAsync(int hotelId, DateTime startDate, DateTime endDate)
        {
            var random = new Random();
            var occupancyData = new List<DailyOccupancyDto>();
            
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
                var occupancy = isWeekend ? random.Next(70, 95) : random.Next(50, 80);
                
                occupancyData.Add(new DailyOccupancyDto
                {
                    Date = date,
                    OccupancyRate = occupancy,
                    AvailableRooms = 200,
                    SoldRooms = (int)(200 * occupancy / 100),
                    AveragePrice = random.Next(100, 300)
                });
            }

            return new OccupancyReportDto
            {
                HotelId = hotelId,
                HotelName = GetHotelName(hotelId),
                StartDate = startDate,
                EndDate = endDate,
                AverageOccupancyRate = occupancyData.Average(o => o.OccupancyRate),
                TotalAvailableRooms = occupancyData.Sum(o => o.AvailableRooms),
                TotalSoldRooms = occupancyData.Sum(o => o.SoldRooms),
                OccupancyData = occupancyData,
                GeneratedAt = DateTime.Now
            };
        }

        public async Task<ReservationReportDto> GetReservationReportAsync(int hotelId, DateTime startDate, DateTime endDate)
        {
            var random = new Random();
            var reservationData = new List<DailyReservationDto>();
            
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                reservationData.Add(new DailyReservationDto
                {
                    Date = date,
                    TotalReservations = random.Next(10, 150),
                    ConfirmedReservations = random.Next(8, 120),
                    CancelledReservations = random.Next(1, 20),
                    NoShowReservations = random.Next(0, 10),
                    AverageLeadDays = random.Next(1, 30)
                });
            }

            return new ReservationReportDto
            {
                HotelId = hotelId,
                HotelName = GetHotelName(hotelId),
                StartDate = startDate,
                EndDate = endDate,
                TotalReservations = reservationData.Sum(r => r.TotalReservations),
                ConfirmedReservations = reservationData.Sum(r => r.ConfirmedReservations),
                CancelledReservations = reservationData.Sum(r => r.CancelledReservations),
                NoShowReservations = reservationData.Sum(r => r.NoShowReservations),
                CancellationRate = (decimal)reservationData.Sum(r => r.CancelledReservations) / reservationData.Sum(r => r.TotalReservations) * 100,
                NoShowRate = (decimal)reservationData.Sum(r => r.NoShowReservations) / reservationData.Sum(r => r.TotalReservations) * 100,
                ReservationData = reservationData,
                GeneratedAt = DateTime.Now
            };
        }

        public async Task<CustomerReportDto> GetCustomerReportAsync(int hotelId, DateTime startDate, DateTime endDate)
        {
            var random = new Random();
            var topCustomers = new List<TopCustomerDto>();
            
            for (int i = 1; i <= 10; i++)
            {
                topCustomers.Add(new TopCustomerDto
                {
                    CustomerId = i,
                    CustomerName = $"Customer {i}",
                    TotalStays = random.Next(1, 50),
                    TotalSpent = random.Next(500, 20000),
                    AverageSpentPerStay = random.Next(100, 1000)
                });
            }

            return new CustomerReportDto
            {
                HotelId = hotelId,
                HotelName = GetHotelName(hotelId),
                StartDate = startDate,
                EndDate = endDate,
                TotalCustomers = random.Next(500, 5000),
                NewCustomers = random.Next(50, 500),
                RepeatCustomers = random.Next(100, 1000),
                CustomerSatisfactionScore = random.Next(70, 98),
                TopCustomers = topCustomers.OrderByDescending(c => c.TotalSpent).ToList(),
                GeneratedAt = DateTime.Now
            };
        }

        public async Task<ChannelReportDto> GetChannelReportAsync(int hotelId, DateTime startDate, DateTime endDate)
        {
            var random = new Random();
            var channelData = new List<ChannelPerformanceDto>
            {
                new ChannelPerformanceDto { ChannelName = "Booking.com", Bookings = random.Next(100, 500), Revenue = random.Next(20000, 100000), Commission = random.Next(2000, 15000) },
                new ChannelPerformanceDto { ChannelName = "Expedia", Bookings = random.Next(80, 400), Revenue = random.Next(15000, 80000), Commission = random.Next(1500, 12000) },
                new ChannelPerformanceDto { ChannelName = "Direct", Bookings = random.Next(50, 300), Revenue = random.Next(10000, 60000), Commission = 0 },
                new ChannelPerformanceDto { ChannelName = "Agoda", Bookings = random.Next(30, 200), Revenue = random.Next(5000, 40000), Commission = random.Next(500, 6000) },
                new ChannelPerformanceDto { ChannelName = "Google Hotels", Bookings = random.Next(20, 150), Revenue = random.Next(3000, 30000), Commission = random.Next(300, 3000) }
            };

            return new ChannelReportDto
            {
                HotelId = hotelId,
                HotelName = GetHotelName(hotelId),
                StartDate = startDate,
                EndDate = endDate,
                TotalBookings = channelData.Sum(c => c.Bookings),
                TotalRevenue = channelData.Sum(c => c.Revenue),
                TotalCommission = channelData.Sum(c => c.Commission),
                NetRevenue = channelData.Sum(c => c.Revenue - c.Commission),
                ChannelData = channelData,
                GeneratedAt = DateTime.Now
            };
        }

        public async Task<byte[]> ExportToExcelAsync(ReportRequestDto request)
        {
            return await _excelExportService.ExportReportAsync(request);
        }

        public async Task<byte[]> ExportToPdfAsync(ReportRequestDto request)
        {
            return await _pdfExportService.ExportReportAsync(request);
        }

        public async Task<ReportDto> GenerateAndSaveReportAsync(ReportRequestDto request, int userId, string userName)
        {
            byte[] fileData;
            string fileExtension;
            
            if (request.Format == "Excel")
            {
                fileData = await ExportToExcelAsync(request);
                fileExtension = "xlsx";
            }
            else
            {
                fileData = await ExportToPdfAsync(request);
                fileExtension = "pdf";
            }

            var report = new Report
            {
                Name = $"{request.ReportType}_Report_{DateTime.Now:yyyyMMdd_HHmmss}",
                Description = $"{request.ReportType} report from {request.StartDate:dd/MM/yyyy} to {request.EndDate:dd/MM/yyyy}",
                ReportType = request.ReportType,
                Format = request.Format,
                HotelId = request.HotelId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                GeneratedAt = DateTime.Now,
                FileSize = fileData.Length,
                Status = "Completed",
                Parameters = System.Text.Json.JsonSerializer.Serialize(request),
                GeneratedByUserId = userId,
                GeneratedByUserName = userName,
                CreatedAt = DateTime.Now
            };

            await _reportRepository.AddAsync(report);
            return _mapper.Map<ReportDto>(report);
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