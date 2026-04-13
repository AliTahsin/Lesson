using Housekeeping.API.Models;

namespace Housekeeping.API.Data
{
    public static class MockData
    {
        private static readonly Random _random = new();

        public static List<HousekeepingTask> GetHousekeepingTasks()
        {
            var tasks = new List<HousekeepingTask>();
            var taskTypes = new[] { "CheckOut", "StayOver", "DeepClean", "Inspection" };
            var priorities = new[] { "High", "Medium", "Low" };
            var statuses = new[] { "Pending", "Assigned", "InProgress", "Completed" };
            
            for (int i = 1; i <= 100; i++)
            {
                var status = statuses[_random.Next(statuses.Length)];
                var createdAt = DateTime.Now.AddHours(-_random.Next(0, 72));
                
                tasks.Add(new HousekeepingTask
                {
                    Id = i,
                    TaskNumber = $"TASK-{DateTime.Now.Year}-{i:D4}",
                    HotelId = _random.Next(1, 4),
                    RoomId = _random.Next(1, 50),
                    RoomNumber = $"{_random.Next(1, 5)}{_random.Next(100, 500)}",
                    TaskType = taskTypes[_random.Next(taskTypes.Length)],
                    Priority = priorities[_random.Next(priorities.Length)],
                    Description = $"{taskTypes[_random.Next(taskTypes.Length)]} task for room",
                    AssignedToStaffId = status != "Pending" ? _random.Next(1, 20) : null,
                    AssignedToStaffName = status != "Pending" ? $"Staff {_random.Next(1, 20)}" : null,
                    CreatedAt = createdAt,
                    ScheduledDate = createdAt.AddHours(_random.Next(1, 24)),
                    StartedAt = status == "InProgress" || status == "Completed" ? createdAt.AddHours(_random.Next(1, 12)) : null,
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
            
            for (int i = 1; i <= 50; i++)
            {
                var status = statuses[_random.Next(statuses.Length)];
                var reportedAt = DateTime.Now.AddDays(-_random.Next(0, 30));
                
                issues.Add(new MaintenanceIssue
                {
                    Id = i,
                    IssueNumber = $"ISS-{DateTime.Now.Year}-{i:D4}",
                    HotelId = _random.Next(1, 4),
                    RoomId = _random.Next(1, 50),
                    RoomNumber = $"{_random.Next(1, 5)}{_random.Next(100, 500)}",
                    Category = categories[_random.Next(categories.Length)],
                    Description = $"{categories[_random.Next(categories.Length)]} issue: {_random.Next(100, 999)}",
                    Priority = priorities[_random.Next(priorities.Length)],
                    ReportedByStaffId = _random.Next(1, 30),
                    ReportedByName = $"Staff {_random.Next(1, 30)}",
                    ReportedAt = reportedAt,
                    AssignedToStaffId = status != "Reported" ? _random.Next(1, 20) : null,
                    AssignedToStaffName = status != "Reported" ? $"Staff {_random.Next(1, 20)}" : null,
                    AssignedAt = status != "Reported" ? reportedAt.AddHours(_random.Next(1, 24)) : null,
                    StartedAt = status == "InProgress" || status == "Resolved" || status == "Closed" ? reportedAt.AddHours(_random.Next(2, 48)) : null,
                    ResolvedAt = status == "Resolved" || status == "Closed" ? reportedAt.AddHours(_random.Next(4, 120)) : null,
                    Status = status,
                    EstimatedCost = _random.Next(50, 2000),
                    ActualCost = status != "Reported" ? _random.Next(50, 2500) : 0
                });
            }
            
            return issues;
        }
    }
}