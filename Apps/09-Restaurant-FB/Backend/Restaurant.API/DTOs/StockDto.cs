namespace Restaurant.API.DTOs
{
    public class StockItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public int CurrentStock { get; set; }
        public int MinStockLevel { get; set; }
        public int ReorderLevel { get; set; }
        public decimal UnitPrice { get; set; }
        public string Supplier { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class CreateStockItemDto
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public int CurrentStock { get; set; }
        public int MinStockLevel { get; set; }
        public int ReorderLevel { get; set; }
        public int ReorderQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Supplier { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class UpdateStockItemDto
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public int MinStockLevel { get; set; }
        public int ReorderLevel { get; set; }
        public int ReorderQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Supplier { get; set; }
    }

    public class StockAlertDto
    {
        public int RestaurantId { get; set; }
        public int AlertCount { get; set; }
        public List<StockAlertItemDto> Alerts { get; set; }
        public DateTime CheckedAt { get; set; }
    }

    public class StockAlertItemDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string AlertType { get; set; }
        public int? CurrentStock { get; set; }
        public int? ReorderLevel { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Message { get; set; }
    }
}