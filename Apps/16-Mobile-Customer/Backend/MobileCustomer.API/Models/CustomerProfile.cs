using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileCustomer.API.Models
{
    public class CustomerProfile
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [MaxLength(100)]
        public string LastName { get; set; }
        
        [MaxLength(200)]
        public string Email { get; set; }
        
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [MaxLength(100)]
        public string Country { get; set; }
        
        [MaxLength(100)]
        public string City { get; set; }
        
        [MaxLength(500)]
        public string Address { get; set; }
        
        [MaxLength(20)]
        public string Language { get; set; } // tr, en, de, ru
        
        public bool BiometricEnabled { get; set; }
        
        public string BiometricKey { get; set; }
        
        public bool PushEnabled { get; set; }
        
        public bool EmailEnabled { get; set; }
        
        public bool SmsEnabled { get; set; }
        
        public string ProfileImageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
        
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}