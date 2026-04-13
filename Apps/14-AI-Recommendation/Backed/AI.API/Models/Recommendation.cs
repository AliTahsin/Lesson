using System.ComponentModel.DataAnnotations;

namespace AI.API.Models
{
    public class Recommendation
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int ItemId { get; set; }
        
        [MaxLength(50)]
        public string ItemType { get; set; }
        
        public decimal Score { get; set; }
        
        [MaxLength(50)]
        public string Algorithm { get; set; } // Collaborative, ContentBased, Popular
        
        public bool IsClicked { get; set; }
        
        public bool IsBooked { get; set; }
        
        public DateTime RecommendedAt { get; set; }
        
        public DateTime? ClickedAt { get; set; }
        
        public DateTime? BookedAt { get; set; }
    }
}