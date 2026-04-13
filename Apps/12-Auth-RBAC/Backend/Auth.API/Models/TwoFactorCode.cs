using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models
{
    public class TwoFactorCode
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }
        
        [MaxLength(50)]
        public string Method { get; set; } // Email, SMS
        
        public bool IsUsed { get; set; }
        
        public DateTime ExpiryDate { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}