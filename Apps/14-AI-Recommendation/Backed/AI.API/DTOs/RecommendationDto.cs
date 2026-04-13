namespace AI.API.DTOs
{
    public class RecommendationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public string ItemName { get; set; }
        public decimal Score { get; set; }
        public string Algorithm { get; set; }
        public string Reason { get; set; }
        public bool IsClicked { get; set; }
        public bool IsBooked { get; set; }
        public DateTime RecommendedAt { get; set; }
    }

    public class TrackInteractionDto
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public string InteractionType { get; set; }
        public int? Rating { get; set; }
        public string Review { get; set; }
    }

    public class RecommendationMetricsDto
    {
        public int TotalRecommendations { get; set; }
        public decimal ClickThroughRate { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal AverageScore { get; set; }
    }
}