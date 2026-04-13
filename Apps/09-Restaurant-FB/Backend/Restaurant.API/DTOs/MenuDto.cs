namespace Restaurant.API.DTOs
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<MenuItemDto> Items { get; set; }
    }

    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public bool IsGlutenFree { get; set; }
        public int PreparationTimeMinutes { get; set; }
        public int Calories { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Allergens { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class CreateMenuItemDto
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public bool IsGlutenFree { get; set; }
        public int PreparationTimeMinutes { get; set; }
        public int Calories { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Allergens { get; set; }
        public string ImageUrl { get; set; }
    }
}