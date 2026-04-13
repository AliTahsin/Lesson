using Staff.API.Models;
using BCrypt.Net;

namespace Staff.API.Data
{
    public static class MockData
    {
        private static readonly Random _random = new();

        public static List<Staff> GetStaffs()
        {
            var staffs = new List<Staff>();
            var roles = new[] { "FrontDesk", "Housekeeping", "Maintenance", "Restaurant" };
            var departments = new[] { "Front Office", "Housekeeping", "Technical", "F&B" };
            var positions = new[] { "Manager", "Supervisor", "Staff", "Intern" };

            // Admin
            staffs.Add(new Staff
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@hotel.com",
                PhoneNumber = "+905551234567",
                PasswordHash = BCrypt.HashPassword("Admin123!"),
                Role = "Admin",
                Department = "Management",
                Position = "System Administrator",
                HotelId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now.AddMonths(-12)
            });

            // Front Desk Staff
            for (int i = 2; i <= 6; i++)
            {
                staffs.Add(new Staff
                {
                    Id = i,
                    FirstName = $"Front{i-1}",
                    LastName = "Staff",
                    Email = $"frontdesk{i-1}@hotel.com",
                    PhoneNumber = $"+90555{_random.Next(1000000, 9999999)}",
                    PasswordHash = BCrypt.HashPassword("Staff123!"),
                    Role = "FrontDesk",
                    Department = "Front Office",
                    Position = i == 2 ? "Manager" : (i == 3 ? "Supervisor" : "Receptionist"),
                    HotelId = _random.Next(1, 4),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-_random.Next(1, 12))
                });
            }

            // Housekeeping Staff
            for (int i = 7; i <= 15; i++)
            {
                staffs.Add(new Staff
                {
                    Id = i,
                    FirstName = $"HK{i-6}",
                    LastName = "Staff",
                    Email = $"housekeeping{i-6}@hotel.com",
                    PhoneNumber = $"+90555{_random.Next(1000000, 9999999)}",
                    PasswordHash = BCrypt.HashPassword("Staff123!"),
                    Role = "Housekeeping",
                    Department = "Housekeeping",
                    Position = i == 7 ? "Manager" : (i == 8 ? "Supervisor" : "Housekeeper"),
                    HotelId = _random.Next(1, 4),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-_random.Next(1, 12))
                });
            }

            // Maintenance Staff
            for (int i = 16; i <= 20; i++)
            {
                staffs.Add(new Staff
                {
                    Id = i,
                    FirstName = $"Tech{i-15}",
                    LastName = "Staff",
                    Email = $"maintenance{i-15}@hotel.com",
                    PhoneNumber = $"+90555{_random.Next(1000000, 9999999)}",
                    PasswordHash = BCrypt.HashPassword("Staff123!"),
                    Role = "Maintenance",
                    Department = "Technical",
                    Position = i == 16 ? "Manager" : "Technician",
                    HotelId = _random.Next(1, 4),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-_random.Next(1, 12))
                });
            }

            // Restaurant Staff
            for (int i = 21; i <= 25; i++)
            {
                staffs.Add(new Staff
                {
                    Id = i,
                    FirstName = $"Rest{i-20}",
                    LastName = "Staff",
                    Email = $"restaurant{i-20}@hotel.com",
                    PhoneNumber = $"+90555{_random.Next(1000000, 9999999)}",
                    PasswordHash = BCrypt.HashPassword("Staff123!"),
                    Role = "Restaurant",
                    Department = "F&B",
                    Position = i == 21 ? "Manager" : (i == 22 ? "Chef" : "Waiter"),
                    HotelId = _random.Next(1, 4),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-_random.Next(1, 12))
                });
            }

            return staffs;
        }

        public static List<StaffTask> GetStaffTasks()
        {
            var tasks = new List<StaffTask>();
            var taskTypes = new[] { "CheckOut", "StayOver", "DeepClean", "Inspection" };
            var priorities = new[] { "High", "Medium", "Low" };
            var statuses = new[] { "Pending", "Assigned", "InProgress", "Completed" };
            var staffIds = Enumerable.Range(7, 9).ToList();

            for (int i = 1; i <= 100; i++)
            {
                var status = statuses[_random.Next(statuses.Length)];
                var createdAt = DateTime.Now.AddHours(-_random.Next(0, 72));
                var assignedStaffId = status != "Pending" ? staffIds[_random.Next(staffIds.Count)] : (int?)null;
                
                tasks.Add(new StaffTask
                {
                    Id = i,
                    TaskNumber = $"TASK-{DateTime.Now.Year}-{i:D4}",
                    HotelId = _random.Next(1, 4),
                    RoomId = _random.Next(1, 100),
                    RoomNumber = $"{_random.Next(1, 5)}{_random.Next(100, 500)}",
                    TaskType = taskTypes[_random.Next(taskTypes.Length)],
                    Priority = priorities[_random.Next(priorities.Length)],
                    Description = $"{taskTypes[_random.Next(taskTypes.Length)]} task for room",
                    AssignedToStaffId = assignedStaffId,
                    AssignedToStaffName = assignedStaffId.HasValue ? $"HK Staff {assignedStaffId}" : null,
                    CreatedAt = createdAt,
                    ScheduledDate = createdAt.AddHours(_random.Next(1, 24)),
                    StartedAt = (status == "InProgress" || status == "Completed") ? createdAt.AddHours(_random.Next(1, 12)) : null,
                    CompletedAt = status == "Completed" ? createdAt.AddHours(_random.Next(2, 24)) : null,
                    Status = status,
                    EstimatedMinutes = _random.Next(15, 60),
                    ActualMinutes = status == "Completed" ? _random.Next(15, 90) : 0
                });
            }

            return tasks;
        }

        public static List<MaintenanceIssue> GetMaintenanceIssues()
        {
            var issues = new List<MaintenanceIssue>();
            var categories = new[] { "Plumbing", "Electrical", "HVAC", "Furniture", "Appliance" };
            var priorities = new[] { "Critical", "High", "Medium", "Low" };
            var statuses = new[] { "Reported", "Assigned", "InProgress", "Resolved", "Closed" };
            var staffIds = Enumerable.Range(16, 5).ToList();

            for (int i = 1; i <= 50; i++)
            {
                var status = statuses[_random.Next(statuses.Length)];
                var reportedAt = DateTime.Now.AddDays(-_random.Next(0, 30));
                var assignedStaffId = status != "Reported" ? staffIds[_random.Next(staffIds.Count)] : (int?)null;

                issues.Add(new MaintenanceIssue
                {
                    Id = i,
                    IssueNumber = $"ISS-{DateTime.Now.Year}-{i:D4}",
                    HotelId = _random.Next(1, 4),
                    RoomId = _random.Next(1, 100),
                    RoomNumber = $"{_random.Next(1, 5)}{_random.Next(100, 500)}",
                    Category = categories[_random.Next(categories.Length)],
                    Description = $"{categories[_random.Next(categories.Length)]} issue: {_random.Next(100, 999)}",
                    Priority = priorities[_random.Next(priorities.Length)],
                    ReportedByStaffId = _random.Next(1, 25),
                    ReportedByName = $"Staff {_random.Next(1, 25)}",
                    ReportedAt = reportedAt,
                    AssignedToStaffId = assignedStaffId,
                    AssignedToStaffName = assignedStaffId.HasValue ? $"Tech Staff {assignedStaffId}" : null,
                    AssignedAt = assignedStaffId.HasValue ? reportedAt.AddHours(_random.Next(1, 24)) : null,
                    StartedAt = (status == "InProgress" || status == "Resolved" || status == "Closed") ? reportedAt.AddHours(_random.Next(2, 48)) : null,
                    ResolvedAt = (status == "Resolved" || status == "Closed") ? reportedAt.AddHours(_random.Next(4, 120)) : null,
                    Status = status,
                    EstimatedCost = _random.Next(50, 2000),
                    ActualCost = status != "Reported" ? _random.Next(50, 2500) : 0
                });
            }

            return issues;
        }

        public static List<CheckInOut> GetCheckInOuts()
        {
            var checks = new List<CheckInOut>();
            var staffIds = Enumerable.Range(2, 5).ToList();

            for (int i = 1; i <= 150; i++)
            {
                var type = _random.Next(0, 10) > 3 ? "CheckIn" : "CheckOut";
                var processedAt = DateTime.Now.AddDays(-_random.Next(0, 30)).AddHours(_random.Next(8, 20));

                checks.Add(new CheckInOut
                {
                    Id = i,
                    ReservationId = 1000 + i,
                    GuestId = _random.Next(1, 100),
                    GuestName = $"Guest {_random.Next(1, 100)}",
                    RoomId = _random.Next(1, 100),
                    RoomNumber = $"{_random.Next(1, 5)}{_random.Next(100, 500)}",
                    ProcessedByStaffId = staffIds[_random.Next(staffIds.Count)],
                    ProcessedByStaffName = $"Front Desk Staff {_random.Next(2, 6)}",
                    Type = type,
                    ProcessedAt = processedAt,
                    Notes = type == "CheckIn" ? "Standard check-in" : "Standard check-out",
                    DigitalKey = type == "CheckIn" ? $"DK-{DateTime.Now.Ticks}-{i}" : null
                });
            }

            return checks;
        }
    }
}