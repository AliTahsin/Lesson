using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Housekeeping.API.Models
{
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(50)]
        public string Category { get; set; }
        
        [MaxLength(50)]
        public string Unit { get; set; }
        
        public int Quantity { get; set; }
        
        public int MinStockLevel { get; set; }
        
        public int MaxStockLevel { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [MaxLength(500)]
        public string Supplier { get; set; }
        
        public DateTime? LastRestockedAt { get; set; }
        
        public bool IsActive { get; set; }
    }
}