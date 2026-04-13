using System.ComponentModel.DataAnnotations;

namespace AI.API.Models
{
    public class SentimentAnalysis
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ReviewId { get; set; }
        
        [MaxLength(50)]
        public string ReviewType { get; set; } // Hotel, Room, Restaurant
        
        [Required]
        [MaxLength(5000)]
        public string ReviewText { get; set; }
        
        [MaxLength(20)]
        public string Sentiment { get; set; } // Positive, Negative, Neutral
        
        public decimal PositiveScore { get; set; }
        
        public decimal NegativeScore { get; set; }
        
        public decimal NeutralScore { get; set; }
        
        public decimal Confidence { get; set; }
        
        public List<string> Keywords { get; set; }
        
        public DateTime AnalyzedAt { get; set; }
        
        public bool IsVerified { get; set; }
    }
}