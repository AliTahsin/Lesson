using System.ComponentModel.DataAnnotations;

namespace AI.API.Models
{
    public class Prediction
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int HotelId { get; set; }
        
        public DateTime PredictionDate { get; set; }
        
        [MaxLength(50)]
        public string PredictionType { get; set; } // Demand, Revenue, Occupancy
        
        public decimal PredictedValue { get; set; }
        
        public decimal ActualValue { get; set; }
        
        public decimal Confidence { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? VerifiedAt { get; set; }
        
        public decimal ErrorRate { get; set; }
        
        [MaxLength(500)]
        public string Features { get; set; } // JSON of features used
    }
}