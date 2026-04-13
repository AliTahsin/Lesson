using System.ComponentModel.DataAnnotations;

namespace Chatbot.API.Models
{
    public class Intent
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(200)]
        public string Description { get; set; }
        
        public List<string> TrainingPhrases { get; set; }
        
        public List<string> Responses { get; set; }
        
        [MaxLength(50)]
        public string Action { get; set; } // BookRoom, CheckStatus, GetInfo, etc.
        
        public decimal ConfidenceThreshold { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}