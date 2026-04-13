using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AI.API.DTOs;
using AI.API.Services;

namespace AI.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SentimentController : ControllerBase
    {
        private readonly ISentimentService _sentimentService;

        public SentimentController(ISentimentService sentimentService)
        {
            _sentimentService = sentimentService;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeSentiment([FromBody] AnalyzeTextDto dto)
        {
            var result = await _sentimentService.AnalyzeSentimentAsync(dto.Text);
            return Ok(result);
        }

        [HttpPost("review")]
        public async Task<IActionResult> AnalyzeReview([FromBody] AnalyzeReviewDto dto)
        {
            var result = await _sentimentService.AnalyzeReviewAsync(dto.ReviewId, dto.ReviewText);
            return Ok(result);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSentimentSummary([FromQuery] int itemId, [FromQuery] string itemType)
        {
            var summary = await _sentimentService.GetSentimentSummaryAsync(itemId, itemType);
            return Ok(summary);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var stats = await _sentimentService.GetSentimentStatisticsAsync(startDate, endDate);
            return Ok(stats);
        }
    }

    public class AnalyzeTextDto
    {
        public string Text { get; set; }
    }

    public class AnalyzeReviewDto
    {
        public int ReviewId { get; set; }
        public string ReviewText { get; set; }
    }
}