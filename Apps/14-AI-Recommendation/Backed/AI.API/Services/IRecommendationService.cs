using AI.API.DTOs;

namespace AI.API.Services
{
    public interface IRecommendationService
    {
        Task<List<RecommendationDto>> GetPersonalizedRecommendationsAsync(int userId, int count = 10);
        Task<List<RecommendationDto>> GetSimilarItemsAsync(int itemId, string itemType, int count = 10);
        Task<List<RecommendationDto>> GetPopularItemsAsync(string itemType, int count = 10);
        Task<RecommendationDto> TrackInteractionAsync(int userId, int itemId, string itemType, string interactionType, int? rating = null);
        Task<RecommendationDto> TrackClickAsync(int recommendationId);
        Task<RecommendationDto> TrackBookingAsync(int recommendationId);
        Task<RecommendationMetricsDto> GetRecommendationMetricsAsync();
    }
}