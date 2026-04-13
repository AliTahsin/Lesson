using System.ComponentModel.DataAnnotations;

namespace Auth.API.DTOs
{
    public class LoginDto
    {
        [Required]
        public string EmailOrUsername { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public string DeviceId { get; set; }
        
        public string DeviceName { get; set; }
        
        public string IpAddress { get; set; }
        
        public string UserAgent { get; set; }
    }

    public class LoginResponseDto
    {
        public bool RequiresTwoFactor { get; set; }
        public int? UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserDto User { get; set; }
        public string Message { get; set; }
    }

    public class RegisterDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
        public string Username { get; set; }
        
        public int HotelId { get; set; }
        
        public string Department { get; set; }
        
        public string Position { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }
        
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
        
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Token { get; set; }
        
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
        
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}