namespace AI.API.DTOs
{
    public class DemandPredictionDto
    {
        public int HotelId { get; set; }
        public DateTime Date { get; set; }
        public int PredictedDemand { get; set; }
        public decimal Confidence { get; set; }
        public List<string> Factors { get; set; }
    }

    public class RevenuePredictionDto
    {
        public int HotelId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PredictedRevenue { get; set; }
        public List<DailyRevenuePredictionDto> DailyPredictions { get; set; }
        public decimal Confidence { get; set; }
    }

    public class DailyRevenuePredictionDto
    {
        public DateTime Date { get; set; }
        public decimal PredictedRevenue { get; set; }
        public decimal Confidence { get; set; }
    }

    public class OccupancyPredictionDto
    {
        public int HotelId { get; set; }
        public DateTime Date { get; set; }
        public decimal PredictedOccupancyRate { get; set; }
        public int PredictedSoldRooms { get; set; }
        public int TotalRooms { get; set; }
        public decimal Confidence { get; set; }
    }

    public class PredictionHistoryDto
    {
        public DateTime Date { get; set; }
        public decimal PredictedValue { get; set; }
        public decimal ActualValue { get; set; }
        public decimal ErrorRate { get; set; }
    }
}