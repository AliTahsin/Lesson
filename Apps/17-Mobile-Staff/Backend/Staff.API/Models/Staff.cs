using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staff.API.Models
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        
        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        [MaxLength(50)]
        public string Role { get; set; } // FrontDesk, Housekeeping, Maintenance, Restaurant
        
        [MaxLength(50)]
        public string Department { get; set; }
        
        [MaxLength(100)]
        public string Position { get; set; }
        
        public int HotelId { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}