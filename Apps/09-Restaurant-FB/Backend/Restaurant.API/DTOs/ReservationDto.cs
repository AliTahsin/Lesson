namespace Restaurant.API.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public string ReservationNumber { get; set; }
        public int RestaurantId { get; set; }
        public string TableNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public int GuestCount { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ReservationTime { get; set; }
        public string SpecialRequests { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReservationDto
    {
        public int RestaurantId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public int GuestCount { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ReservationTime { get; set; }
        public string SpecialRequests { get; set; }
    }
}