using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MICE.API.Models
{
    public class Equipment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int HotelId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(50)]
        public string Category { get; set; } // Audio, Visual, Furniture, Other
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal DailyPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal WeeklyPrice { get; set; }
        
        public int TotalQuantity { get; set; }
        
        public int AvailableQuantity { get; set; }
        
        public bool IsActive { get; set; }
        
        public string ImageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}