using System.ComponentModel.DataAnnotations;

namespace Staff.API.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int StaffId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Body { get; set; }
        
        [MaxLength(50)]
        public string Type { get; set; } // Task, Issue, System
        
        public int? RelatedId { get; set; }
        
        public bool IsRead { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? ReadAt { get; set; }
    }
}