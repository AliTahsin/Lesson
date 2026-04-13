namespace Auth.API.DTOs
{
    public class TwoFactorVerifyDto
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MinLength(6)]
        [MaxLength(6)]
        public string Code { get; set; }
        
        public string DeviceId { get; set; }
        
        public string DeviceName { get; set; }
        
        public string IpAddress { get; set; }
        
        public string UserAgent { get; set; }
    }

    public class TwoFactorEnableDto
    {
        public bool Enabled { get; set; }
        public string Method { get; set; } // Email, SMS, Authenticator
    }

    public class TwoFactorSendCodeDto
    {
        [Required]
        public int UserId { get; set; }
    }
}