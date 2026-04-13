using DynamicPricing.API.Models;
using System.Collections.Generic;

namespace DynamicPricing.API.Data
{
    public static class MockData
    {
        public static List<PriceRule> GetPriceRules()
        {
            return new List<PriceRule>
            {
                // Occupancy based rules
                new PriceRule 
                { 
                    Id = 1, Name = "Very Low Occupancy", RuleType = "Occupancy", 
                    MinValue = 0, MaxValue = 30, Multiplier = 0.85m, FixedAdjustment = 0, 
                    Priority = 1, IsActive = true, 
                    Description = "Düşük doluluk - %15 indirim" 
                },
                new PriceRule 
                { 
                    Id = 2, Name = "Low Occupancy", RuleType = "Occupancy", 
                    MinValue = 30, MaxValue = 50, Multiplier = 0.95m, FixedAdjustment = 0, 
                    Priority = 1, IsActive = true, 
                    Description = "Orta altı doluluk - %5 indirim" 
                },
                new PriceRule 
                { 
                    Id = 3, Name = "Normal Occupancy", RuleType = "Occupancy", 
                    MinValue = 50, MaxValue = 70, Multiplier = 1.00m, FixedAdjustment = 0, 
                    Priority = 1, IsActive = true, 
                    Description = "Normal doluluk - fiyat sabit" 
                },
                new PriceRule 
                { 
                    Id = 4, Name = "High Occupancy", RuleType = "Occupancy", 
                    MinValue = 70, MaxValue = 85, Multiplier = 1.15m, FixedAdjustment = 0, 
                    Priority = 1, IsActive = true, 
                    Description = "Yüksek doluluk - %15 zam" 
                },
                new PriceRule 
                { 
                    Id = 5, Name = "Very High Occupancy", RuleType = "Occupancy", 
                    MinValue = 85, MaxValue = 100, Multiplier = 1.30m, FixedAdjustment = 0, 
                    Priority = 1, IsActive = true, 
                    Description = "Çok yüksek doluluk - %30 zam" 
                },
                
                // Demand based rules
                new PriceRule 
                { 
                    Id = 6, Name = "Very Low Demand", RuleType = "Demand", 
                    MinValue = 0, MaxValue = 20, Multiplier = 0.90m, FixedAdjustment = 0, 
                    Priority = 2, IsActive = true, 
                    Description = "Çok düşük talep - %10 indirim" 
                },
                new PriceRule 
                { 
                    Id = 7, Name = "Low Demand", RuleType = "Demand", 
                    MinValue = 20, MaxValue = 40, Multiplier = 0.95m, FixedAdjustment = 0, 
                    Priority = 2, IsActive = true, 
                    Description = "Düşük talep - %5 indirim" 
                },
                new PriceRule 
                { 
                    Id = 8, Name = "Normal Demand", RuleType = "Demand", 
                    MinValue = 40, MaxValue = 60, Multiplier = 1.00m, FixedAdjustment = 0, 
                    Priority = 2, IsActive = true, 
                    Description = "Normal talep - fiyat sabit" 
                },
                new PriceRule 
                { 
                    Id = 9, Name = "High Demand", RuleType = "Demand", 
                    MinValue = 60, MaxValue = 80, Multiplier = 1.10m, FixedAdjustment = 0, 
                    Priority = 2, IsActive = true, 
                    Description = "Yüksek talep - %10 zam" 
                },
                new PriceRule 
                { 
                    Id = 10, Name = "Very High Demand", RuleType = "Demand", 
                    MinValue = 80, MaxValue = 100, Multiplier = 1.25m, FixedAdjustment = 0, 
                    Priority = 2, IsActive = true, 
                    Description = "Çok yüksek talep - %25 zam" 
                },
                
                // Competitor based rules
                new PriceRule 
                { 
                    Id = 11, Name = "Below Competitor", RuleType = "Competitor", 
                    MinValue = 0, MaxValue = 80, Multiplier = 0.95m, FixedAdjustment = -10, 
                    Priority = 3, IsActive = true, 
                    Description = "Rakip altı - %5 indirim + 10€ düşük" 
                },
                new PriceRule 
                { 
                    Id = 12, Name = "Match Competitor", RuleType = "Competitor", 
                    MinValue = 80, MaxValue = 120, Multiplier = 1.00m, FixedAdjustment = 0, 
                    Priority = 3, IsActive = true, 
                    Description = "Rakip ile aynı" 
                },
                new PriceRule 
                { 
                    Id = 13, Name = "Above Competitor", RuleType = "Competitor", 
                    MinValue = 120, MaxValue = 200, Multiplier = 1.05m, FixedAdjustment = 15, 
                    Priority = 3, IsActive = true, 
                    Description = "Rakip üstü - %5 zam + 15€ yüksek" 
                }
            };
        }

        public static List<Season> GetSeasons()
        {
            return new List<Season>
            {
                new Season 
                { 
                    Id = 1, HotelId = 0, Name = "Low Season", 
                    StartMonth = 11, EndMonth = 2, StartDay = 1, EndDay = 28, 
                    Multiplier = 0.85m, Color = "#16a34a" 
                },
                new Season 
                { 
                    Id = 2, HotelId = 0, Name = "Mid Season", 
                    StartMonth = 3, EndMonth = 5, StartDay = 1, EndDay = 31, 
                    Multiplier = 1.00m, Color = "#f59e0b" 
                },
                new Season 
                { 
                    Id = 3, HotelId = 0, Name = "High Season", 
                    StartMonth = 6, EndMonth = 8, StartDay = 1, EndDay = 31, 
                    Multiplier = 1.30m, Color = "#dc2626" 
                },
                new Season 
                { 
                    Id = 4, HotelId = 0, Name = "Peak Season", 
                    StartMonth = 9, EndMonth = 10, StartDay = 1, EndDay = 31, 
                    Multiplier = 1.20m, Color = "#ea580c" 
                },
                // Special dates
                new Season 
                { 
                    Id = 5, HotelId = 1, Name = "New Year", 
                    StartMonth = 12, EndMonth = 1, StartDay = 30, EndDay = 2, 
                    Multiplier = 2.00m, Color = "#7c3aed" 
                },
                new Season 
                { 
                    Id = 6, HotelId = 2, Name = "Summer Festival", 
                    StartMonth = 7, EndMonth = 7, StartDay = 15, EndDay = 25, 
                    Multiplier = 1.50m, Color = "#ec4899" 
                }
            };
        }

        public static List<DemandFactor> GetDemandFactors(DateTime startDate, DateTime endDate)
        {
            var factors = new List<DemandFactor>();
            var random = new Random();
            var hotels = new[] { 1, 2, 3 };

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                foreach (var hotelId in hotels)
                {
                    var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
                    var isSummer = date.Month >= 6 && date.Month <= 8;
                    
                    var baseDemand = isWeekend ? 70 : 50;
                    baseDemand += isSummer ? 20 : 0;
                    baseDemand += random.Next(-10, 15);
                    
                    factors.Add(new DemandFactor
                    {
                        Id = factors.Count + 1,
                        HotelId = hotelId,
                        Date = date,
                        DemandScore = Math.Min(100, Math.Max(0, baseDemand)),
                        ExpectedOccupancy = Math.Min(100, Math.Max(0, baseDemand - random.Next(0, 20))),
                        WebSearchCount = random.Next(100, 5000),
                        BookingAttempts = random.Next(10, 200),
                        Events = date.Month == 7 && date.Day > 10 && date.Day < 20 ? new List<string> { "Music Festival" } : new List<string>(),
                        Notes = ""
                    });
                }
            }
            return factors;
        }

        public static List<DynamicPrice> GetDynamicPrices(DateTime startDate, DateTime endDate)
        {
            var prices = new List<DynamicPrice>();
            var random = new Random();
            var rooms = new[] { 1, 2, 3, 4, 5, 6, 7 };
            var basePrices = new Dictionary<int, decimal>
            {
                { 1, 150 }, { 2, 250 }, { 3, 450 }, { 4, 120 }, { 5, 200 }, { 6, 180 }, { 7, 500 }
            };

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                foreach (var roomId in rooms)
                {
                    var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
                    var seasonMultiplier = GetSeasonMultiplier(date, 0);
                    var demandMultiplier = GetDemandMultiplier(date, random.Next(30, 95));
                    var occupancyMultiplier = GetOccupancyMultiplier(random.Next(20, 95));
                    
                    var basePrice = basePrices[roomId];
                    var calculatedPrice = basePrice * occupancyMultiplier;
                    var finalPrice = calculatedPrice * demandMultiplier * seasonMultiplier;
                    
                    if (isWeekend)
                        finalPrice *= 1.10m;
                    
                    prices.Add(new DynamicPrice
                    {
                        Id = prices.Count + 1,
                        RoomId = roomId,
                        Date = date,
                        BasePrice = basePrice,
                        CalculatedPrice = Math.Round(calculatedPrice, 2),
                        OccupancyMultiplier = occupancyMultiplier,
                        DemandMultiplier = demandMultiplier,
                        SeasonMultiplier = seasonMultiplier,
                        CompetitorMultiplier = 1.00m,
                        SpecialEventMultiplier = 1.00m,
                        FinalPrice = Math.Round(finalPrice, 2),
                        IsActive = true,
                        LastUpdated = DateTime.Now
                    });
                }
            }
            return prices;
        }

        private static decimal GetSeasonMultiplier(DateTime date, int hotelId)
        {
            var seasons = GetSeasons().Where(s => s.HotelId == 0 || s.HotelId == hotelId);
            
            foreach (var season in seasons)
            {
                if (season.StartMonth <= season.EndMonth)
                {
                    if (date.Month >= season.StartMonth && date.Month <= season.EndMonth &&
                        date.Day >= season.StartDay && date.Day <= season.EndDay)
                        return season.Multiplier;
                }
                else // Year wrap (e.g., Dec to Jan)
                {
                    if ((date.Month >= season.StartMonth && date.Day >= season.StartDay) ||
                        (date.Month <= season.EndMonth && date.Day <= season.EndDay))
                        return season.Multiplier;
                }
            }
            return 1.00m;
        }

        private static decimal GetDemandMultiplier(DateTime date, int demandScore)
        {
            if (demandScore >= 80) return 1.25m;
            if (demandScore >= 60) return 1.10m;
            if (demandScore >= 40) return 1.00m;
            if (demandScore >= 20) return 0.95m;
            return 0.90m;
        }

        private static decimal GetOccupancyMultiplier(int occupancyRate)
        {
            if (occupancyRate >= 85) return 1.30m;
            if (occupancyRate >= 70) return 1.15m;
            if (occupancyRate >= 50) return 1.00m;
            if (occupancyRate >= 30) return 0.95m;
            return 0.85m;
        }
    }
}