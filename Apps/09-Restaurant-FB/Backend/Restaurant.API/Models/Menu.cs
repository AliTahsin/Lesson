using System.ComponentModel.DataAnnotations;

namespace Restaurant.API.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int RestaurantId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Breakfast, Lunch, Dinner, Kids, Drinks
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime ValidFrom { get; set; }
        
        public DateTime? ValidTo { get; set; }
        
        public List<MenuItem> Items { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }

    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int MenuId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(50)]
        public string Category { get; set; } // Appetizer, Main, Dessert, Beverage
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        [MaxLength(3)]
        public string Currency { get; set; } = "EUR";
        
        public bool IsVegetarian { get; set; }
        
        public bool IsVegan { get; set; }
        
        public bool IsGlutenFree { get; set; }
        
        public int PreparationTimeMinutes { get; set; }
        
        public int Calories { get; set; }
        
        public List<string> Ingredients { get; set; }
        
        public List<string> Allergens { get; set; }
        
        public string ImageUrl { get; set; }
        
        public bool IsAvailable { get; set; }
        
        public int StockQuantity { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}