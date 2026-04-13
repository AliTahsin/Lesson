using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MICE.API.Models
{
    public class CateringOrder
    {
        [Key]
        public int Id { get; set; }
        
        public int EventId { get; set; }
        
        [MaxLength(50)]
        public string MealType { get; set; } // Breakfast, Lunch, Dinner, CoffeeBreak
        
        public int AttendeeCount { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerPerson { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        
        [MaxLength(500)]
        public string MenuDetails { get; set; }
        
        [MaxLength(500)]
        public string DietaryNotes { get; set; }
        
        public DateTime ServiceTime { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}