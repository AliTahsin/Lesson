using AI.API.Models;
using AI.API.DTOs;
using AI.API.Repositories;

namespace AI.API.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly IInteractionRepository _interactionRepository;
        private readonly ILogger<PredictionService> _logger;

        public PredictionService(
            IInteractionRepository interactionRepository,
            ILogger<PredictionService> logger)
        {
            _interactionRepository = interactionRepository;
            _logger = logger;
        }

        public async Task<DemandPredictionDto> PredictDemandAsync(int hotelId, DateTime date)
        {
            // Simulate demand prediction using historical data
            var random = new Random();
            var historicalInteractions = await _interactionRepository.GetByDateRangeAsync(date.AddDays(-90), date.AddDays(-1));
            var historicalCount = historicalInteractions.Count(i => i.ItemId == hotelId && i.ItemType == "Hotel");
            
            var baseDemand = historicalCount / 90; // Average daily demand
            var dayOfWeek = date.DayOfWeek;
            var isWeekend = dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
            var isHoliday = date.Month == 12 && date.Day >= 20; // Holiday season
            
            var multiplier = 1.0m;
            if (isWeekend) multiplier += 0.2m;
            if (isHoliday) multiplier += 0.5m;
            
            var predictedDemand = baseDemand * multiplier;
            var confidence = 0.7m + (random.Next(-10, 10) / 100m);
            
            return new DemandPredictionDto
            {
                HotelId = hotelId,
                Date = date,
                PredictedDemand = (int)Math.Round(predictedDemand),
                Confidence = Math.Max(0.5m, Math.Min(0.95m, confidence)),
                Factors = new List<string>
                {
                    isWeekend ? "Weekend factor" : null,
                    isHoliday ? "Holiday season factor" : null,
                    $"Historical demand: {baseDemand:F0} average"
                }.Where(f => f != null).ToList()
            };
        }

        public async Task<RevenuePredictionDto> PredictRevenueAsync(int hotelId, DateTime startDate, DateTime endDate)
        {
            var random = new Random();
            var days = (endDate - startDate).Days + 1;
            var predictedRevenue = 0m;
            var dailyPredictions = new List<DailyRevenuePredictionDto>();
            
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var demandPrediction = await PredictDemandAsync(hotelId, date);
                var avgPrice = random.Next(100, 300);
                var dailyRevenue = demandPrediction.PredictedDemand * avgPrice;
                predictedRevenue += dailyRevenue;
                
                dailyPredictions.Add(new DailyRevenuePredictionDto
                {
                    Date = date,
                    PredictedRevenue = dailyRevenue,
                    Confidence = demandPrediction.Confidence
                });
            }
            
            return new RevenuePredictionDto
            {
                HotelId = hotelId,
                StartDate = startDate,
                EndDate = endDate,
                PredictedRevenue = predictedRevenue,
                DailyPredictions = dailyPredictions,
                Confidence = dailyPredictions.Average(d => d.Confidence)
            };
        }

        public async Task<OccupancyPredictionDto> PredictOccupancyAsync(int hotelId, DateTime date)
        {
            var demandPrediction = await PredictDemandAsync(hotelId, date);
            var totalRooms = 200; // Mock total rooms
            var predictedOccupancy = (decimal)demandPrediction.PredictedDemand / totalRooms * 100;
            
            return new OccupancyPredictionDto
            {
                HotelId = hotelId,
                Date = date,
                PredictedOccupancyRate = Math.Min(100, predictedOccupancy),
                PredictedSoldRooms = demandPrediction.PredictedDemand,
                TotalRooms = totalRooms,
                Confidence = demandPrediction.Confidence
            };
        }

        public async Task<List<PredictionHistoryDto>> GetPredictionHistoryAsync(int hotelId, string predictionType)
        {
            // Simulate prediction history
            var history = new List<PredictionHistoryDto>();
            var random = new Random();
            
            for (int i = 0; i < 30; i++)
            {
                var date = DateTime.Now.AddDays(-i);
                var predicted = random.Next(50, 150);
                var actual = predicted + random.Next(-30, 30);
                
                history.Add(new PredictionHistoryDto
                {
                    Date = date,
                    PredictedValue = predicted,
                    ActualValue = Math.Max(0, actual),
                    ErrorRate = Math.Abs((decimal)(actual - predicted) / predicted) * 100
                });
            }
            
            return history.OrderBy(h => h.Date).ToList();
        }

        public async Task<bool> ValidatePredictionAsync(int predictionId, decimal actualValue)
        {
            // Simulate validation
            await Task.Delay(100);
            return true;
        }
    }
}