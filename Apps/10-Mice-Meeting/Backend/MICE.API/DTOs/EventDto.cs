namespace MICE.API.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string EventNumber { get; set; }
        public int HotelId { get; set; }
        public int MeetingRoomId { get; set; }
        public string MeetingRoomName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EventType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ExpectedAttendees { get; set; }
        public int ActualAttendees { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal ActualCost { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCompany { get; set; }
        public string SpecialRequests { get; set; }
        public List<EventScheduleDto> Schedule { get; set; }
        public List<int> EquipmentIds { get; set; }
    }

    public class EventScheduleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Speaker { get; set; }
        public string Location { get; set; }
        public int Order { get; set; }
    }

    public class CreateEventDto
    {
        public int HotelId { get; set; }
        public int MeetingRoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EventType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ExpectedAttendees { get; set; }
        public decimal TotalBudget { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerCompany { get; set; }
        public string SpecialRequests { get; set; }
        public List<CreateEventScheduleDto> Schedule { get; set; }
        public List<int> EquipmentIds { get; set; }
    }

    public class CreateEventScheduleDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Speaker { get; set; }
        public string Location { get; set; }
    }

    public class UpdateEventDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ExpectedAttendees { get; set; }
        public decimal TotalBudget { get; set; }
        public string SpecialRequests { get; set; }
    }
}