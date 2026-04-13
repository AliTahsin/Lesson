namespace MobileCustomer.API.DTOs
{
    public class SpaServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CreateSpaAppointmentDto
    {
        public int ServiceId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string SpecialRequests { get; set; }
    }

    public class SpaAppointmentDto
    {
        public int Id { get; set; }
        public string AppointmentNumber { get; set; }
        public int UserId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string SpecialRequests { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
    }
}