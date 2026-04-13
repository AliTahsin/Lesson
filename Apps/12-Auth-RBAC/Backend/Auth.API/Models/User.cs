using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.API.Models
{
    public class User
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
        public string Username { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsEmailVerified { get; set; }
        
        public bool IsPhoneVerified { get; set; }
        
        public bool TwoFactorEnabled { get; set; }
        
        [MaxLength(50)]
        public string TwoFactorMethod { get; set; } // Email, SMS, Authenticator
        
        public DateTime? LastLoginAt { get; set; }
        
        public DateTime? PasswordChangedAt { get; set; }
        
        public int FailedLoginAttempts { get; set; }
        
        public DateTime? LockoutEnd { get; set; }
        
        public int HotelId { get; set; } // Hangi otelde çalışıyor
        
        [MaxLength(50)]
        public string Department { get; set; } // FrontDesk, Housekeeping, F&B, Management
        
        [MaxLength(100)]
        public string Position { get; set; }
        
        public string ProfileImageUrl { get; set; }
        
        public List<int> RoleIds { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? LastActivityAt { get; set; }
        
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        
        [NotMapped]
        public List<string> Permissions { get; set; }
    }
}