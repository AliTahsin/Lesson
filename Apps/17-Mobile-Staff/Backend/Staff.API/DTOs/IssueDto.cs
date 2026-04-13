namespace Staff.API.DTOs
{
    public class IssueDto
    {
        public int Id { get; set; }
        public string IssueNumber { get; set; }
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string ReportedByName { get; set; }
        public DateTime ReportedAt { get; set; }
        public string AssignedToStaffName { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string Status { get; set; }
        public string ResolutionNotes { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal ActualCost { get; set; }
    }

    public class CreateIssueDto
    {
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public int? ReportedByStaffId { get; set; }
        public string ReportedByName { get; set; }
        public List<string> Images { get; set; }
    }

    public class ResolveIssueDto
    {
        public string ResolutionNotes { get; set; }
        public decimal ActualCost { get; set; }
    }
}