using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AI.API.DTOs;
using AI.API.Services;

namespace AI.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationsController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpGet("personalized")]
        public async Task<IActionResult> GetPersonalizedRecommendations([FromQuery] int count = 10)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var recommendations = await _recommendationService.GetPersonalizedRecommendationsAsync(userId, count);
            return Ok(recommendations);
        }

        [HttpGet("similar/{itemId}")]
        public async Task<IActionResult> GetSimilarItems(int itemId, [FromQuery] string itemType, [FromQuery] int count = 10)
        {
            var recommendations = await _recommendationService.GetSimilarItemsAsync(itemId, itemType, count);
            return Ok(recommendations);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularItems([FromQuery] string itemType, [FromQuery] int count = 10)
        {
            var recommendations = await _recommendationService.GetPopularItemsAsync(itemType, count);
            return Ok(recommendations);
        }

        [HttpPost("track")]
        public async Task<IActionResult> TrackInteraction([FromBody] TrackInteractionDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var result = await _recommendationService.TrackInteractionAsync(
                userId, dto.ItemId, dto.ItemType, dto.InteractionType, dto.Rating);
            return Ok(result);
        }

        [HttpPost("{id}/click")]
        public async Task<IActionResult> TrackClick(int id)
        {
            var result = await _recommendationService.TrackClickAsync(id);
            return Ok(result);
        }

        [HttpPost("{id}/book")]
        public async Task<IActionResult> TrackBooking(int id)
        {
            var result = await _recommendationService.TrackBookingAsync(id);
            return Ok(result);
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics()
        {
            var metrics = await _recommendationService.GetRecommendationMetricsAsync();
            return Ok(metrics);
        }
    }
}