using DynamicPricing.API.Models;
using DynamicPricing.API.DTOs;
using DynamicPricing.API.Data;

namespace DynamicPricing.API.Services
{
    public class PricingEngineService
    {
        private readonly List<PriceRule> _rules;
        private readonly List<Season> _seasons;
        private List<DemandFactor> _demandFactors;
        private List<DynamicPrice> _dynamicPrices;

        public PricingEngineService()
        {
            _rules = MockData.GetPriceRules();
            _seasons = MockData.GetSeasons();
            _demandFactors = MockData.GetDemandFactors(DateTime.Today, DateTime.Today.AddDays(90));
            _dynamicPrices = MockData.GetDynamicPrices(DateTime.Today, DateTime.Today.AddDays(90));
        }

        // Calculate price for a single room on a specific date
        public PriceResponseDto CalculatePrice(PriceRequestDto request)
        {
            var result = new PriceResponseDto
            {
                RoomId = request.RoomId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NightCount = (request.CheckOutDate - request.CheckInDate).Days,
                DailyPrices = new List<DailyPriceDto>()
            };

            decimal totalPrice = 0;

            for (var date = request.CheckInDate; date < request.CheckOutDate; date = date.AddDays(1))
            {
                var dailyPrice = CalculateDailyPrice(request.RoomId, date, request.GuestCount);
                result.DailyPrices.Add(dailyPrice);
                totalPrice += dailyPrice.FinalPrice;
            }

            result.TotalPrice = Math.Round(totalPrice, 2);
            result.AveragePricePerNight = Math.Round(totalPrice / result.NightCount, 2);
            
            // Apply discounts for longer stays
            if (result.NightCount >= 7)
            {
                result.LongStayDiscount = 0.10m;
                result.TotalPrice = Math.Round(result.TotalPrice * 0.90m, 2);
            }
            else if (result.NightCount >= 3)
            {
                result.LongStayDiscount = 0.05m;
                result.TotalPrice = Math.Round(result.TotalPrice * 0.95m, 2);
            }

            return result;
        }

        // Calculate daily price
        private DailyPriceDto CalculateDailyPrice(int roomId, DateTime date, int guestCount)
        {
            var dynamicPrice = _dynamicPrices.FirstOrDefault(dp => dp.RoomId == roomId && dp.Date.Date == date.Date);
            
            if (dynamicPrice == null)
            {
                // Fallback calculation
                dynamicPrice = new DynamicPrice
                {
                    RoomId = roomId,
                    Date = date,
                    BasePrice = GetBasePrice(roomId),
                    FinalPrice = GetBasePrice(roomId)
                };
            }

            var demandFactor = _demandFactors.FirstOrDefault(df => df.HotelId == GetHotelId(roomId) && df.Date.Date == date.Date);
            var demandScore = demandFactor?.DemandScore ?? 50;
            
            // Recalculate with latest data
            var occupancyRule = GetApplicableRule("Occupancy", GetOccupancyRate(roomId, date));
            var demandRule = GetApplicableRule("Demand", demandScore);
            var seasonMultiplier = GetSeasonMultiplier(date, GetHotelId(roomId));

            var price = dynamicPrice.BasePrice;
            var breakdowns = new List<PriceBreakdownDto>();

            // Apply occupancy multiplier
            if (occupancyRule != null)
            {
                var newPrice = price * occupancyRule.Multiplier;
                breakdowns.Add(new PriceBreakdownDto
                {
                    Factor = "Doluluk Oranı",
                    Description = occupancyRule.Description,
                    Multiplier = occupancyRule.Multiplier,
                    Impact = newPrice - price
                });
                price = newPrice;
            }

            // Apply demand multiplier
            if (demandRule != null)
            {
                var newPrice = price * demandRule.Multiplier;
                breakdowns.Add(new PriceBreakdownDto
                {
                    Factor = "Talep Durumu",
                    Description = demandRule.Description,
                    Multiplier = demandRule.Multiplier,
                    Impact = newPrice - price
                });
                price = newPrice;
            }

            // Apply season multiplier
            if (seasonMultiplier != 1.00m)
            {
                var newPrice = price * seasonMultiplier;
                breakdowns.Add(new PriceBreakdownDto
                {
                    Factor = "Sezon",
                    Description = GetSeasonName(date, GetHotelId(roomId)),
                    Multiplier = seasonMultiplier,
                    Impact = newPrice - price
                });
                price = newPrice;
            }

            // Weekend surcharge
            var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
            if (isWeekend)
            {
                var newPrice = price * 1.10m;
                breakdowns.Add(new PriceBreakdownDto
                {
                    Factor = "Hafta Sonu",
                    Description = "Hafta sonu ek ücreti",
                    Multiplier = 1.10m,
                    Impact = newPrice - price
                });
                price = newPrice;
            }

            // Extra guest charge
            var baseCapacity = GetRoomCapacity(roomId);
            if (guestCount > baseCapacity)
            {
                var extraGuests = guestCount - baseCapacity;
                var extraCharge = price * 0.20m * extraGuests;
                breakdowns.Add(new PriceBreakdownDto
                {
                    Factor = "Ekstra Misafir",
                    Description = $"{extraGuests} ekstra misafir",
                    Multiplier = 1 + (0.20m * extraGuests),
                    Impact = extraCharge
                });
                price += extraCharge;
            }

            return new DailyPriceDto
            {
                Date = date,
                BasePrice = dynamicPrice.BasePrice,
                FinalPrice = Math.Round(price, 2),
                Breakdowns = breakdowns,
                DemandScore = demandScore,
                OccupancyRate = GetOccupancyRate(roomId, date)
            };
        }

        private PriceRule GetApplicableRule(string ruleType, decimal value)
        {
            return _rules.FirstOrDefault(r => 
                r.RuleType == ruleType && 
                r.IsActive && 
                value >= r.MinValue && 
                value < r.MaxValue);
        }

        private decimal GetSeasonMultiplier(DateTime date, int hotelId)
        {
            var season = _seasons.FirstOrDefault(s => 
                (s.HotelId == 0 || s.HotelId == hotelId) &&
                IsDateInSeason(date, s));
            return season?.Multiplier ?? 1.00m;
        }

        private string GetSeasonName(DateTime date, int hotelId)
        {
            var season = _seasons.FirstOrDefault(s => 
                (s.HotelId == 0 || s.HotelId == hotelId) &&
                IsDateInSeason(date, s));
            return season?.Name ?? "Normal Sezon";
        }

        private bool IsDateInSeason(DateTime date, Season season)
        {
            if (season.StartMonth <= season.EndMonth)
            {
                if (date.Month == season.StartMonth && date.Day < season.StartDay) return false;
                if (date.Month == season.EndMonth && date.Day > season.EndDay) return false;
                return date.Month >= season.StartMonth && date.Month <= season.EndMonth;
            }
            else // Year wrap
            {
                return date.Month >= season.StartMonth || date.Month <= season.EndMonth;
            }
        }

        // Helper methods (mock data)
        private decimal GetBasePrice(int roomId)
        {
            var prices = new Dictionary<int, decimal>
            {
                { 1, 150 }, { 2, 250 }, { 3, 450 }, { 4, 120 }, { 5, 200 }, { 6, 180 }, { 7, 500 }
            };
            return prices.ContainsKey(roomId) ? prices[roomId] : 150;
        }

        private int GetHotelId(int roomId)
        {
            if (roomId <= 3) return 1;
            if (roomId <= 5) return 2;
            return 3;
        }

        private int GetRoomCapacity(int roomId)
        {
            if (roomId == 3 || roomId == 7) return 4;
            if (roomId == 2 || roomId == 5) return 3;
            return 2;
        }

        private decimal GetOccupancyRate(int roomId, DateTime date)
        {
            var random = new Random(roomId * date.DayOfYear);
            return random.Next(20, 95);
        }

        // Get all dynamic prices
        public List<DynamicPrice> GetAllDynamicPrices()
        {
            return _dynamicPrices;
        }

        // Get dynamic prices for a room
        public List<DynamicPrice> GetDynamicPricesByRoom(int roomId, DateTime startDate, DateTime endDate)
        {
            return _dynamicPrices
                .Where(dp => dp.RoomId == roomId && dp.Date >= startDate && dp.Date <= endDate)
                .OrderBy(dp => dp.Date)
                .ToList();
        }

        // Get demand factors
        public List<DemandFactor> GetDemandFactors(int hotelId, DateTime startDate, DateTime endDate)
        {
            return _demandFactors
                .Where(df => df.HotelId == hotelId && df.Date >= startDate && df.Date <= endDate)
                .OrderBy(df => df.Date)
                .ToList();
        }

        // Get all price rules
        public List<PriceRule> GetAllPriceRules()
        {
            return _rules;
        }

        // Get all seasons
        public List<Season> GetAllSeasons()
        {
            return _seasons;
        }

        // Update dynamic price
        public DynamicPrice UpdateDynamicPrice(int roomId, DateTime date, decimal price)
        {
            var dynamicPrice = _dynamicPrices.FirstOrDefault(dp => dp.RoomId == roomId && dp.Date.Date == date.Date);
            if (dynamicPrice != null)
            {
                dynamicPrice.FinalPrice = price;
                dynamicPrice.LastUpdated = DateTime.Now;
            }
            return dynamicPrice;
        }

        // Get pricing statistics
        public object GetPricingStatistics(int hotelId, DateTime startDate, DateTime endDate)
        {
            var prices = _dynamicPrices
                .Where(dp => GetHotelId(dp.RoomId) == hotelId && dp.Date >= startDate && dp.Date <= endDate)
                .ToList();

            var demandFactors = _demandFactors
                .Where(df => df.HotelId == hotelId && df.Date >= startDate && df.Date <= endDate)
                .ToList();

            return new
            {
                AveragePrice = prices.Any() ? prices.Average(p => p.FinalPrice) : 0,
                MinPrice = prices.Any() ? prices.Min(p => p.FinalPrice) : 0,
                MaxPrice = prices.Any() ? prices.Max(p => p.FinalPrice) : 0,
                AverageDemandScore = demandFactors.Any() ? demandFactors.Average(df => df.DemandScore) : 0,
                PriceVolatility = CalculateVolatility(prices),
                ByDayOfWeek = prices.GroupBy(p => p.Date.DayOfWeek).Select(g => new
                {
                    Day = g.Key.ToString(),
                    AveragePrice = g.Average(p => p.FinalPrice),
                    Count = g.Count()
                }),
                RevenueForecast = prices.Sum(p => p.FinalPrice * 0.7m) // 70% expected occupancy
            };
        }

        private decimal CalculateVolatility(List<DynamicPrice> prices)
        {
            if (prices.Count < 2) return 0;
            
            var changes = new List<decimal>();
            for (int i = 1; i < prices.Count; i++)
            {
                var change = Math.Abs(prices[i].FinalPrice - prices[i-1].FinalPrice) / prices[i-1].FinalPrice;
                changes.Add(change);
            }
            return changes.Average();
        }
    }
}