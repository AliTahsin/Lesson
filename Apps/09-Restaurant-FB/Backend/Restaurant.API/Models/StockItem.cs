using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.API.Models
{
    public class StockItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int RestaurantId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(50)]
        public string Category { get; set; }
        
        [MaxLength(50)]
        public string Unit { get; set; } // kg, lt, piece, box
        
        public int CurrentStock { get; set; }
        
        public int MinStockLevel { get; set; }
        
        public int MaxStockLevel { get; set; }
        
        public int ReorderLevel { get; set; }
        
        public int ReorderQuantity { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [MaxLength(100)]
        public string Supplier { get; set; }
        
        public DateTime? LastRestockedAt { get; set; }
        
        public DateTime? ExpiryDate { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}