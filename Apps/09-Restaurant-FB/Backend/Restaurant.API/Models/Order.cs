using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; }
        
        [Required]
        public int RestaurantId { get; set; }
        
        public int? TableId { get; set; }
        
        [MaxLength(50)]
        public string TableNumber { get; set; }
        
        public int? CustomerId { get; set; }
        
        [MaxLength(100)]
        public string CustomerName { get; set; }
        
        public int? RoomId { get; set; }
        
        [MaxLength(50)]
        public string RoomNumber { get; set; }
        
        [MaxLength(50)]
        public string OrderType { get; set; } // DineIn, Takeaway, RoomService
        
        [MaxLength(50)]
        public string Status { get; set; } // Pending, Preparing, Ready, Served, Completed, Cancelled
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }
        
        public decimal TaxRate { get; set; } = 10;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        [MaxLength(3)]
        public string Currency { get; set; } = "EUR";
        
        [MaxLength(500)]
        public string SpecialInstructions { get; set; }
        
        public DateTime OrderTime { get; set; }
        
        public DateTime? PreparationStartTime { get; set; }
        
        public DateTime? ReadyTime { get; set; }
        
        public DateTime? ServedTime { get; set; }
        
        public DateTime? CompletedTime { get; set; }
        
        public List<OrderItem> Items { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        
        public int MenuItemId { get; set; }
        
        [MaxLength(100)]
        public string ItemName { get; set; }
        
        public int Quantity { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        
        [MaxLength(500)]
        public string SpecialInstructions { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Pending, Preparing, Ready, Served
    }
}