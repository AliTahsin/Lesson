namespace MobileCustomer.API.DTOs
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Language { get; set; }
        public bool BiometricEnabled { get; set; }
        public bool PushEnabled { get; set; }
        public bool EmailEnabled { get; set; }
        public bool SmsEnabled { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public class UpdateProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ProfileImageUrl { get; set; }
    }

    public class NotificationSettingsDto
    {
        public bool PushEnabled { get; set; }
        public bool EmailEnabled { get; set; }
        public bool SmsEnabled { get; set; }
    }

    public class RegisterDeviceDto
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Platform { get; set; }
        public string OsVersion { get; set; }
        public string AppVersion { get; set; }
        public string PushToken { get; set; }
    }

    public class DeviceInfoDto
    {
        public int UserId { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Platform { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastActiveAt { get; set; }
    }
}