namespace Auth.API.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string TwoFactorMethod { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int HotelId { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string ProfileImageUrl { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
    }

    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string ProfileImageUrl { get; set; }
    }

    public class UpdateUserRolesDto
    {
        public List<int> RoleIds { get; set; }
    }
}