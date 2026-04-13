using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileCustomer.API.Models
{
    public class RoomServiceOrder
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int ReservationId { get; set; }
        
        [Required]
        public int RoomId { get; set; }
        
        [MaxLength(50)]
        public string RoomNumber { get; set; }
        
        public List<OrderItem> Items { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        [MaxLength(3)]
        public string Currency { get; set; } = "EUR";
        
        [MaxLength(500)]
        public string SpecialInstructions { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Pending, Preparing, OnTheWay, Delivered, Cancelled
        
        public DateTime OrderTime { get; set; }
        
        public DateTime? EstimatedDeliveryTime { get; set; }
        
        public DateTime? DeliveredAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        
        [MaxLength(200)]
        public string ItemName { get; set; }
        
        public int Quantity { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        
        [MaxLength(500)]
        public string SpecialInstructions { get; set; }
    }
}