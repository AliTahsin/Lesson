namespace Restaurant.API.DTOs
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HotelId { get; set; }
        public string CuisineType { get; set; }
        public string Description { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public int TotalTables { get; set; }
        public int TotalCapacity { get; set; }
        public decimal AveragePricePerPerson { get; set; }
        public bool IsActive { get; set; }
        public List<string> Images { get; set; }
    }

    public class CreateRestaurantDto
    {
        public string Name { get; set; }
        public int HotelId { get; set; }
        public string CuisineType { get; set; }
        public string Description { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public int TotalTables { get; set; }
        public int TotalCapacity { get; set; }
        public decimal AveragePricePerPerson { get; set; }
        public List<string> Images { get; set; }
    }

    public class UpdateRestaurantDto
    {
        public string Name { get; set; }
        public string CuisineType { get; set; }
        public string Description { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public decimal AveragePricePerPerson { get; set; }
        public bool IsActive { get; set; }
    }

    public class TableDto
    {
        public int Id { get; set; }
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string QrCodeUrl { get; set; }
    }
}