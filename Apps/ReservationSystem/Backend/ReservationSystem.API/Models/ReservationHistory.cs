namespace ReservationSystem.API.Models
{
    public class ReservationHistory
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public string Action { get; set; } // Created, Modified, Cancelled, CheckedIn, CheckedOut
        public string Description { get; set; }
        public string PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}