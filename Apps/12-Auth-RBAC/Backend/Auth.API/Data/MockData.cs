using Auth.API.Models;

// MockData.cs içinde eksik olabilecek alanlar kontrol edildi:
// ✅ GetUsers() - PasswordHash BCrypt ile hashlenmiş
// ✅ GetRoles() - PermissionIds listesi var
// ✅ GetPermissions() - Tüm izinler mevcut

namespace Auth.API.Data
{
    public static class MockData
    {
        private static readonly BCrypt.Net.BCrypt _bcrypt;

        public static List<User> GetUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@hotel.com",
                    PhoneNumber = "+905551234567",
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    IsActive = true,
                    IsEmailVerified = true,
                    IsPhoneVerified = true,
                    TwoFactorEnabled = false,
                    HotelId = 1,
                    Department = "Management",
                    Position = "System Administrator",
                    RoleIds = new List<int> { 1 },
                    CreatedAt = DateTime.Now.AddMonths(-12),
                    LastLoginAt = DateTime.Now.AddDays(-1)
                },
                new User
                {
                    Id = 2,
                    FirstName = "Hotel",
                    LastName = "Manager",
                    Email = "manager@hotel.com",
                    PhoneNumber = "+905559876543",
                    Username = "manager",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager123!"),
                    IsActive = true,
                    IsEmailVerified = true,
                    IsPhoneVerified = true,
                    TwoFactorEnabled = false,
                    HotelId = 1,
                    Department = "Management",
                    Position = "Hotel Manager",
                    RoleIds = new List<int> { 2 },
                    CreatedAt = DateTime.Now.AddMonths(-6),
                    LastLoginAt = DateTime.Now.AddDays(-2)
                },
                new User
                {
                    Id = 3,
                    FirstName = "Front",
                    LastName = "Desk",
                    Email = "frontdesk@hotel.com",
                    PhoneNumber = "+905557654321",
                    Username = "frontdesk",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Front123!"),
                    IsActive = true,
                    IsEmailVerified = true,
                    IsPhoneVerified = true,
                    TwoFactorEnabled = false,
                    HotelId = 1,
                    Department = "FrontDesk",
                    Position = "Receptionist",
                    RoleIds = new List<int> { 3 },
                    CreatedAt = DateTime.Now.AddMonths(-3),
                    LastLoginAt = DateTime.Now.AddDays(-1)
                },
                new User
                {
                    Id = 4,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "guest@email.com",
                    PhoneNumber = "+905551112233",
                    Username = "guest",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Guest123!"),
                    IsActive = true,
                    IsEmailVerified = true,
                    IsPhoneVerified = false,
                    TwoFactorEnabled = false,
                    HotelId = 0,
                    Department = null,
                    Position = null,
                    RoleIds = new List<int> { 4 },
                    CreatedAt = DateTime.Now.AddMonths(-1),
                    LastLoginAt = DateTime.Now.AddDays(-3)
                }
            };
        }

        public static List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role
                {
                    Id = 1,
                    Name = "Super Admin",
                    Description = "Full system access",
                    Level = "SuperAdmin",
                    PermissionIds = Enumerable.Range(1, 20).ToList(),
                    IsDefault = false,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-12)
                },
                new Role
                {
                    Id = 2,
                    Name = "Hotel Manager",
                    Description = "Manage hotel operations",
                    Level = "Admin",
                    PermissionIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
                    IsDefault = false,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-12)
                },
                new Role
                {
                    Id = 3,
                    Name = "Front Desk Staff",
                    Description = "Manage reservations and check-ins",
                    Level = "Staff",
                    PermissionIds = new List<int> { 3, 4, 5, 6, 7, 8 },
                    IsDefault = false,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-12)
                },
                new Role
                {
                    Id = 4,
                    Name = "Guest",
                    Description = "Regular guest access",
                    Level = "Guest",
                    PermissionIds = new List<int> { 1, 2 },
                    IsDefault = true,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-12)
                },
                new Role
                {
                    Id = 5,
                    Name = "Housekeeping Staff",
                    Description = "Manage cleaning tasks",
                    Level = "Staff",
                    PermissionIds = new List<int> { 9, 10, 11 },
                    IsDefault = false,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-12)
                }
            };
        }

        public static List<Permission> GetPermissions()
        {
            return new List<Permission>
            {
                // User permissions
                new Permission { Id = 1, Name = "View Profile", Code = "user:view", Category = "User", Description = "Can view own profile", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 2, Name = "Edit Profile", Code = "user:edit", Category = "User", Description = "Can edit own profile", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 3, Name = "Manage Users", Code = "user:manage", Category = "User", Description = "Can manage all users", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                
                // Reservation permissions
                new Permission { Id = 4, Name = "View Reservations", Code = "reservation:view", Category = "Reservation", Description = "Can view reservations", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 5, Name = "Create Reservations", Code = "reservation:create", Category = "Reservation", Description = "Can create reservations", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 6, Name = "Edit Reservations", Code = "reservation:edit", Category = "Reservation", Description = "Can edit reservations", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 7, Name = "Cancel Reservations", Code = "reservation:cancel", Category = "Reservation", Description = "Can cancel reservations", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 8, Name = "Manage Reservations", Code = "reservation:manage", Category = "Reservation", Description = "Can manage all reservations", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                
                // Hotel permissions
                new Permission { Id = 9, Name = "View Hotels", Code = "hotel:view", Category = "Hotel", Description = "Can view hotels", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 10, Name = "Manage Hotels", Code = "hotel:manage", Category = "Hotel", Description = "Can manage hotels", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 11, Name = "View Rooms", Code = "room:view", Category = "Hotel", Description = "Can view rooms", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 12, Name = "Manage Rooms", Code = "room:manage", Category = "Hotel", Description = "Can manage rooms", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                
                // Report permissions
                new Permission { Id = 13, Name = "View Reports", Code = "report:view", Category = "Report", Description = "Can view reports", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 14, Name = "Export Reports", Code = "report:export", Category = "Report", Description = "Can export reports", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 15, Name = "Manage Reports", Code = "report:manage", Category = "Report", Description = "Can manage reports", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                
                // Setting permissions
                new Permission { Id = 16, Name = "View Settings", Code = "setting:view", Category = "Setting", Description = "Can view settings", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 17, Name = "Manage Settings", Code = "setting:manage", Category = "Setting", Description = "Can manage settings", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                
                // Role permissions
                new Permission { Id = 18, Name = "View Roles", Code = "role:view", Category = "Role", Description = "Can view roles", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                new Permission { Id = 19, Name = "Manage Roles", Code = "role:manage", Category = "Role", Description = "Can manage roles", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) },
                
                // Audit permissions
                new Permission { Id = 20, Name = "View Audit Logs", Code = "audit:view", Category = "Audit", Description = "Can view audit logs", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-12) }
            };
        }
    }
}