using AutoMapper;
using AI.API.Models;
using AI.API.DTOs;
using AI.API.Repositories;

namespace AI.API.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IInteractionRepository _interactionRepository;
        private readonly IRecommendationRepository _recommendationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RecommendationService> _logger;

        public RecommendationService(
            IInteractionRepository interactionRepository,
            IRecommendationRepository recommendationRepository,
            IMapper mapper,
            ILogger<RecommendationService> logger)
        {
            _interactionRepository = interactionRepository;
            _recommendationRepository = recommendationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<RecommendationDto>> GetPersonalizedRecommendationsAsync(int userId, int count = 10)
        {
            // Get user interactions
            var interactions = await _interactionRepository.GetUserBehaviorAsync(userId, 90);
            
            // Get item ratings
            var hotelRatings = await _interactionRepository.GetItemRatingsAsync("Hotel");
            var roomRatings = await _interactionRepository.GetItemRatingsAsync("Room");
            var restaurantRatings = await _interactionRepository.GetItemRatingsAsync("Restaurant");
            
            // Collaborative filtering based on user's rated items
            var recommendations = new List<RecommendationDto>();
            
            // Find similar users (users who rated similar items)
            var similarUsers = await FindSimilarUsersAsync(userId, interactions);
            
            // Get items liked by similar users
            var recommendedItemIds = await GetItemsFromSimilarUsersAsync(similarUsers, interactions);
            
            // Create recommendation objects
            foreach (var itemId in recommendedItemIds.Take(count))
            {
                recommendations.Add(new RecommendationDto
                {
                    UserId = userId,
                    ItemId = itemId.Key,
                    ItemType = itemId.Type,
                    Score = itemId.Score,
                    Algorithm = "CollaborativeFiltering",
                    Reason = GetRecommendationReason(itemId.Type)
                });
            }
            
            // If not enough recommendations, add popular items
            if (recommendations.Count < count)
            {
                var popularItems = await GetPopularItemsAsync("Hotel", count - recommendations.Count);
                recommendations.AddRange(popularItems);
            }
            
            return recommendations;
        }

        public async Task<List<RecommendationDto>> GetSimilarItemsAsync(int itemId, string itemType, int count = 10)
        {
            // Get interactions for this item
            var itemInteractions = await _interactionRepository.GetByItemAsync(itemId, itemType);
            
            // Find users who interacted with this item
            var userIds = itemInteractions.Select(i => i.UserId).Distinct();
            
            // Get items those users also liked
            var similarItems = new Dictionary<int, (string Type, decimal Score)>();
            
            foreach (var userId in userIds)
            {
                var userInteractions = await _interactionRepository.GetByUserAsync(userId);
                foreach (var interaction in userInteractions)
                {
                    if (interaction.ItemId != itemId && interaction.Rating.HasValue && interaction.Rating >= 4)
                    {
                        var key = interaction.ItemId;
                        var type = interaction.ItemType;
                        
                        if (!similarItems.ContainsKey(key))
                        {
                            similarItems[key] = (type, 0);
                        }
                        similarItems[key] = (type, similarItems[key].Score + (interaction.Rating.Value / 5m));
                    }
                }
            }
            
            var recommendations = similarItems
                .OrderByDescending(s => s.Value.Score)
                .Take(count)
                .Select(s => new RecommendationDto
                {
                    ItemId = s.Key,
                    ItemType = s.Value.Type,
                    Score = s.Value.Score,
                    Algorithm = "ContentBased",
                    Reason = "Similar items based on your preferences"
                })
                .ToList();
            
            return recommendations;
        }

        public async Task<List<RecommendationDto>> GetPopularItemsAsync(string itemType, int count = 10)
        {
            var topRecommendations = await _recommendationRepository.GetTopRecommendationsAsync(itemType, count);
            return _mapper.Map<List<RecommendationDto>>(topRecommendations);
        }

        public async Task<RecommendationDto> TrackInteractionAsync(int userId, int itemId, string itemType, string interactionType, int? rating = null)
        {
            var interaction = new UserInteraction
            {
                UserId = userId,
                ItemId = itemId,
                ItemType = itemType,
                InteractionType = interactionType,
                Rating = rating,
                InteractionDate = DateTime.Now
            };
            
            await _interactionRepository.AddAsync(interaction);
            
            // Generate recommendation based on this interaction
            var recommendation = new Recommendation
            {
                UserId = userId,
                ItemId = itemId,
                ItemType = itemType,
                Score = rating.HasValue ? rating.Value / 5m : 0.5m,
                Algorithm = "CollaborativeFiltering",
                RecommendedAt = DateTime.Now
            };
            
            await _recommendationRepository.AddAsync(recommendation);
            
            return _mapper.Map<RecommendationDto>(recommendation);
        }

        public async Task<RecommendationDto> TrackClickAsync(int recommendationId)
        {
            await _recommendationRepository.MarkAsClickedAsync(recommendationId);
            var recommendation = await _recommendationRepository.GetByIdAsync(recommendationId);
            return _mapper.Map<RecommendationDto>(recommendation);
        }

        public async Task<RecommendationDto> TrackBookingAsync(int recommendationId)
        {
            await _recommendationRepository.MarkAsBookedAsync(recommendationId);
            var recommendation = await _recommendationRepository.GetByIdAsync(recommendationId);
            return _mapper.Map<RecommendationDto>(recommendation);
        }

        public async Task<RecommendationMetricsDto> GetRecommendationMetricsAsync()
        {
            var recommendations = await _recommendationRepository.GetTopRecommendationsAsync("Hotel", 1000);
            
            return new RecommendationMetricsDto
            {
                TotalRecommendations = recommendations.Count,
                ClickThroughRate = recommendations.Any() ? (decimal)recommendations.Count(r => r.IsClicked) / recommendations.Count * 100 : 0,
                ConversionRate = recommendations.Any() ? (decimal)recommendations.Count(r => r.IsBooked) / recommendations.Count * 100 : 0,
                AverageScore = recommendations.Any() ? recommendations.Average(r => r.Score) : 0
            };
        }

        private async Task<List<int>> FindSimilarUsersAsync(int userId, List<UserInteraction> userInteractions)
        {
            // Simplified: find users who rated same items
            var userItemIds = userInteractions.Where(i => i.Rating.HasValue).Select(i => i.ItemId).Distinct();
            
            var allInteractions = await _interactionRepository.GetByDateRangeAsync(DateTime.Now.AddDays(-90), DateTime.Now);
            var similarUsers = allInteractions
                .Where(i => i.UserId != userId && i.Rating.HasValue && userItemIds.Contains(i.ItemId))
                .Select(i => i.UserId)
                .Distinct()
                .ToList();
            
            return similarUsers;
        }

        private async Task<List<(int ItemId, string Type, decimal Score)>> GetItemsFromSimilarUsersAsync(List<int> similarUsers, List<UserInteraction> currentUserInteractions)
        {
            var recommendedItems = new Dictionary<int, (string Type, decimal Score, int Count)>();
            var currentUserItemIds = currentUserInteractions.Select(i => i.ItemId).ToHashSet();
            
            foreach (var userId in similarUsers)
            {
                var userInteractions = await _interactionRepository.GetByUserAsync(userId);
                foreach (var interaction in userInteractions.Where(i => i.Rating.HasValue && i.Rating >= 4))
                {
                    if (!currentUserItemIds.Contains(interaction.ItemId))
                    {
                        if (!recommendedItems.ContainsKey(interaction.ItemId))
                        {
                            recommendedItems[interaction.ItemId] = (interaction.ItemType, 0, 0);
                        }
                        var item = recommendedItems[interaction.ItemId];
                        recommendedItems[interaction.ItemId] = (item.Type, item.Score + interaction.Rating.Value, item.Count + 1);
                    }
                }
            }
            
            return recommendedItems
                .Select(i => (i.Key, i.Value.Type, i.Value.Score / i.Value.Count))
                .OrderByDescending(i => i.Score)
                .ToList();
        }

        private string GetRecommendationReason(string itemType)
        {
            return itemType switch
            {
                "Hotel" => "Based on your previous stays",
                "Room" => "Recommended for you",
                "Restaurant" => "Popular among similar guests",
                _ => "Personalized recommendation"
            };
        }
    }
}