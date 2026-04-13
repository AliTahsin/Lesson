namespace Restaurant.API.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int RestaurantId { get; set; }
        public int? TableId { get; set; }
        public string TableNumber { get; set; }
        public string CustomerName { get; set; }
        public string OrderType { get; set; }
        public string Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public string SpecialInstructions { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime? PreparationStartTime { get; set; }
        public DateTime? ReadyTime { get; set; }
        public DateTime? ServedTime { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string SpecialInstructions { get; set; }
        public string Status { get; set; }
    }

    public class CreateOrderDto
    {
        public int RestaurantId { get; set; }
        public int? TableId { get; set; }
        public string TableNumber { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string OrderType { get; set; }
        public string SpecialInstructions { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
    }

    public class CreateOrderItemDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string SpecialInstructions { get; set; }
    }

    public class DailyRevenueDto
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    public class PopularItemsDto
    {
        public string Period { get; set; }
        public List<PopularItemDto> Items { get; set; }
    }

    public class PopularItemDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}