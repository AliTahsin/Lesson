using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.API.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        public int HotelId { get; set; }
        
        [MaxLength(50)]
        public string CuisineType { get; set; } // Turkish, Italian, Chinese, etc.
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(20)]
        public string OpeningTime { get; set; }
        
        [MaxLength(20)]
        public string ClosingTime { get; set; }
        
        public int TotalTables { get; set; }
        
        public int TotalCapacity { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal AveragePricePerPerson { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsBreakfastAvailable { get; set; }
        
        public bool IsLunchAvailable { get; set; }
        
        public bool IsDinnerAvailable { get; set; }
        
        public List<string> Images { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}