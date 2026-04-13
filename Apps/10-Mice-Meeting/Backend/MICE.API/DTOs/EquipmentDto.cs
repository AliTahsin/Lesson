namespace MICE.API.DTOs
{
    public class EquipmentDto
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal WeeklyPrice { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CreateEquipmentDto
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal WeeklyPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string ImageUrl { get; set; }
    }

    public class UpdateEquipmentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal WeeklyPrice { get; set; }
        public bool IsActive { get; set; }
    }
}