using System.ComponentModel.DataAnnotations;

namespace AI.API.Models
{
    public class UserInteraction
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int ItemId { get; set; }
        
        [MaxLength(50)]
        public string ItemType { get; set; } // Hotel, Room, Restaurant, Event
        
        [MaxLength(50)]
        public string InteractionType { get; set; } // View, Click, Book, Favorite, Review
        
        public int? Rating { get; set; } // 1-5
        
        [MaxLength(1000)]
        public string Review { get; set; }
        
        public DateTime InteractionDate { get; set; }
        
        [MaxLength(50)]
        public string DeviceType { get; set; }
        
        [MaxLength(50)]
        public string SessionId { get; set; }
        
        public int DurationSeconds { get; set; }
        
        public Dictionary<string, string> Metadata { get; set; }
    }
}