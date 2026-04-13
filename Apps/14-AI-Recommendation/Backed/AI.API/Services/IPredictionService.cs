using AI.API.DTOs;

namespace AI.API.Services
{
    public interface IPredictionService
    {
        Task<DemandPredictionDto> PredictDemandAsync(int hotelId, DateTime date);
        Task<RevenuePredictionDto> PredictRevenueAsync(int hotelId, DateTime startDate, DateTime endDate);
        Task<OccupancyPredictionDto> PredictOccupancyAsync(int hotelId, DateTime date);
        Task<List<PredictionHistoryDto>> GetPredictionHistoryAsync(int hotelId, string predictionType);
        Task<bool> ValidatePredictionAsync(int predictionId, decimal actualValue);
    }
}