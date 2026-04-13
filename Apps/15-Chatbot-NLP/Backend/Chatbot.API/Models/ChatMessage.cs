using System.ComponentModel.DataAnnotations;

namespace Chatbot.API.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ConversationId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string SenderType { get; set; } // User, Bot, Agent
        
        public int? SenderId { get; set; }
        
        [MaxLength(100)]
        public string SenderName { get; set; }
        
        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }
        
        [MaxLength(500)]
        public string Intent { get; set; }
        
        [MaxLength(500)]
        public string Entities { get; set; } // JSON string
        
        [MaxLength(50)]
        public string Status { get; set; } // Sent, Delivered, Read
        
        public DateTime SentAt { get; set; }
        
        public DateTime? DeliveredAt { get; set; }
        
        public DateTime? ReadAt { get; set; }
    }
}