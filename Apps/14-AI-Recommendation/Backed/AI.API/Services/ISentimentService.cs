using AI.API.DTOs;

namespace AI.API.Services
{
    public interface ISentimentService
    {
        Task<SentimentResultDto> AnalyzeSentimentAsync(string text);
        Task<SentimentResultDto> AnalyzeReviewAsync(int reviewId, string reviewText);
        Task<List<SentimentSummaryDto>> GetSentimentSummaryAsync(int itemId, string itemType);
        Task<SentimentStatisticsDto> GetSentimentStatisticsAsync(DateTime startDate, DateTime endDate);
    }
}