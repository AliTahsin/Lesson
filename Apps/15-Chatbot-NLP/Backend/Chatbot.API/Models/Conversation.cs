using System.ComponentModel.DataAnnotations;

namespace Chatbot.API.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        
        public int? UserId { get; set; }
        
        [MaxLength(100)]
        public string UserEmail { get; set; }
        
        [MaxLength(100)]
        public string UserName { get; set; }
        
        public int? HotelId { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Active, Resolved, Transferred
        
        public int? AssignedAgentId { get; set; }
        
        [MaxLength(100)]
        public string AssignedAgentName { get; set; }
        
        public DateTime StartedAt { get; set; }
        
        public DateTime? EndedAt { get; set; }
        
        public DateTime? LastMessageAt { get; set; }
        
        public int MessageCount { get; set; }
        
        public bool IsBotActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}