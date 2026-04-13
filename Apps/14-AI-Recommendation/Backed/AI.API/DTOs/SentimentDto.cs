namespace AI.API.DTOs
{
    public class SentimentResultDto
    {
        public int? ReviewId { get; set; }
        public string Sentiment { get; set; }
        public decimal PositiveScore { get; set; }
        public decimal NegativeScore { get; set; }
        public decimal NeutralScore { get; set; }
        public decimal Confidence { get; set; }
        public List<string> Keywords { get; set; }
        public DateTime AnalyzedAt { get; set; }
    }

    public class SentimentSummaryDto
    {
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public int TotalReviews { get; set; }
        public int PositiveCount { get; set; }
        public int NegativeCount { get; set; }
        public decimal PositivePercentage { get; set; }
        public decimal NegativePercentage { get; set; }
        public decimal AverageRating { get; set; }
    }

    public class SentimentStatisticsDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalReviews { get; set; }
        public int PositiveReviews { get; set; }
        public int NegativeReviews { get; set; }
        public decimal AverageSentimentScore { get; set; }
        public List<string> TopPositiveKeywords { get; set; }
        public List<string> TopNegativeKeywords { get; set; }
    }
}