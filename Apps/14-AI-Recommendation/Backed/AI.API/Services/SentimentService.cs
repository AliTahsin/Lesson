using AI.API.DTOs;
using System.Text.RegularExpressions;

namespace AI.API.Services
{
    public class SentimentService : ISentimentService
    {
        private readonly ILogger<SentimentService> _logger;

        // Turkish positive and negative word lists (simplified)
        private static readonly HashSet<string> PositiveWords = new()
        {
            "harika", "mükemmel", "güzel", "iyi", "süper", "başarılı", "temiz", "konforlu",
            "lezzetli", "yardımsever", "profesyonel", "hızlı", "rahat", "keyifli", "muhteşem"
        };
        
        private static readonly HashSet<string> NegativeWords = new()
        {
            "kötü", "berbat", "pis", "geç", "yavaş", "ilgisiz", "pahalı", "sorunlu",
            "kırık", "bozuk", "yetersiz", "hayal kırıklığı", "rahatsız", "gürültülü"
        };

        public SentimentService(ILogger<SentimentService> logger)
        {
            _logger = logger;
        }

        public async Task<SentimentResultDto> AnalyzeSentimentAsync(string text)
        {
            var result = await Task.Run(() => AnalyzeText(text));
            return result;
        }

        public async Task<SentimentResultDto> AnalyzeReviewAsync(int reviewId, string reviewText)
        {
            var result = await AnalyzeSentimentAsync(reviewText);
            result.ReviewId = reviewId;
            return result;
        }

        public async Task<List<SentimentSummaryDto>> GetSentimentSummaryAsync(int itemId, string itemType)
        {
            // Simulate sentiment summary for an item
            var random = new Random();
            var totalReviews = random.Next(50, 500);
            var positiveCount = (int)(totalReviews * random.Next(60, 90) / 100m);
            var negativeCount = totalReviews - positiveCount;
            
            return new List<SentimentSummaryDto>
            {
                new SentimentSummaryDto
                {
                    ItemId = itemId,
                    ItemType = itemType,
                    TotalReviews = totalReviews,
                    PositiveCount = positiveCount,
                    NegativeCount = negativeCount,
                    PositivePercentage = (decimal)positiveCount / totalReviews * 100,
                    NegativePercentage = (decimal)negativeCount / totalReviews * 100,
                    AverageRating = random.Next(3, 5) + (decimal)random.Next(0, 10) / 10
                }
            };
        }

        public async Task<SentimentStatisticsDto> GetSentimentStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            // Simulate sentiment statistics
            var random = new Random();
            var totalReviews = random.Next(1000, 5000);
            var positiveReviews = (int)(totalReviews * random.Next(70, 90) / 100m);
            var negativeReviews = totalReviews - positiveReviews;
            
            return new SentimentStatisticsDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalReviews = totalReviews,
                PositiveReviews = positiveReviews,
                NegativeReviews = negativeReviews,
                AverageSentimentScore = (decimal)positiveReviews / totalReviews * 100,
                TopPositiveKeywords = new List<string> { "temiz", "konforlu", "yardımsever", "lezzetli" },
                TopNegativeKeywords = new List<string> { "geç", "pahalı", "gürültülü" }
            };
        }

        private SentimentResultDto AnalyzeText(string text)
        {
            var lowerText = text.ToLower();
            var words = Regex.Split(lowerText, @"\W+");
            
            var positiveCount = words.Count(w => PositiveWords.Contains(w));
            var negativeCount = words.Count(w => NegativeWords.Contains(w));
            
            var totalSentiment = positiveCount + negativeCount;
            var positiveScore = totalSentiment > 0 ? (decimal)positiveCount / totalSentiment : 0.5m;
            var negativeScore = totalSentiment > 0 ? (decimal)negativeCount / totalSentiment : 0.5m;
            var neutralScore = 1 - positiveScore - negativeScore;
            
            string sentiment;
            if (positiveScore > 0.6m)
                sentiment = "Positive";
            else if (negativeScore > 0.4m)
                sentiment = "Negative";
            else
                sentiment = "Neutral";
            
            var keywords = words
                .Where(w => PositiveWords.Contains(w) || NegativeWords.Contains(w))
                .Distinct()
                .Take(10)
                .ToList();
            
            return new SentimentResultDto
            {
                Sentiment = sentiment,
                PositiveScore = positiveScore,
                NegativeScore = negativeScore,
                NeutralScore = neutralScore,
                Confidence = Math.Max(positiveScore, Math.Max(negativeScore, neutralScore)),
                Keywords = keywords,
                AnalyzedAt = DateTime.Now
            };
        }
    }
}