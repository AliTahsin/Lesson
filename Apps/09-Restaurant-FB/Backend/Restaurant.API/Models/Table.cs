using System.ComponentModel.DataAnnotations;

namespace Restaurant.API.Models
{
    public class Table
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int RestaurantId { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string TableNumber { get; set; }
        
        public int Capacity { get; set; }
        
        [MaxLength(50)]
        public string Location { get; set; } // Indoor, Outdoor, Terrace, Private
        
        public bool IsSmoking { get; set; }
        
        public bool HasView { get; set; }
        
        public bool IsActive { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } // Available, Occupied, Reserved, Cleaning
        
        public string QrCodeUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}