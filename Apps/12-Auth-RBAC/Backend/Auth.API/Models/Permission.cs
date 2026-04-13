using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Code { get; set; } // user:view, user:create, reservation:manage
        
        [MaxLength(50)]
        public string Category { get; set; } // User, Reservation, Hotel, Report, Setting
        
        [MaxLength(200)]
        public string Description { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}