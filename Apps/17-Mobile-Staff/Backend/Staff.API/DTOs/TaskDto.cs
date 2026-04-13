namespace Staff.API.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string TaskNumber { get; set; }
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string TaskType { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public int? AssignedToStaffId { get; set; }
        public string AssignedToStaffName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int EstimatedMinutes { get; set; }
        public int ActualMinutes { get; set; }
    }

    public class CreateTaskDto
    {
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string TaskType { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public int? EstimatedMinutes { get; set; }
        public string Notes { get; set; }
    }

    public class CompleteTaskDto
    {
        public string Notes { get; set; }
        public List<string> AfterImages { get; set; }
    }
}