namespace MobileCustomer.API.DTOs
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public int PreparationTimeMinutes { get; set; }
    }

    public class CreateOrderItemDto
    {
        public int MenuItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string SpecialInstructions { get; set; }
    }

    public class CreateRoomServiceOrderDto
    {
        public int ReservationId { get; set; }
        public string RoomNumber { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
        public string SpecialInstructions { get; set; }
    }

    public class OrderItemDto
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string SpecialInstructions { get; set; }
    }

    public class RoomServiceOrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int UserId { get; set; }
        public int ReservationId { get; set; }
        public string RoomNumber { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public string SpecialInstructions { get; set; }
        public string Status { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class OrderStatusDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public List<OrderStepDto> Steps { get; set; }
    }

    public class OrderStepDto
    {
        public string Step { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? Time { get; set; }
    }
}