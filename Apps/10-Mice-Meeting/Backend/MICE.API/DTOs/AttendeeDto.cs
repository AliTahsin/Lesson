namespace MICE.API.DTOs
{
    public class AttendeeDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public DateTime? CheckInTime { get; set; }
        public bool HasCheckedIn { get; set; }
        public string DietaryRestrictions { get; set; }
        public string Status { get; set; }
        public string QrCode { get; set; }
    }

    public class CreateAttendeeDto
    {
        public int EventId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string DietaryRestrictions { get; set; }
        public string SpecialNeeds { get; set; }
    }

    public class UpdateAttendeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string DietaryRestrictions { get; set; }
        public string SpecialNeeds { get; set; }
    }

    public class AttendeeStatisticsDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int TotalRegistered { get; set; }
        public int CheckedIn { get; set; }
        public decimal CheckedInRate { get; set; }
        public int NoShow { get; set; }
        public List<CompanyStatDto> ByCompany { get; set; }
    }

    public class CompanyStatDto
    {
        public string Company { get; set; }
        public int Count { get; set; }
        public int CheckedIn { get; set; }
    }
}