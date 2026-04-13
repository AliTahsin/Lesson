using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MICE.API.Models
{
    public class MeetingRoom
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int HotelId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(50)]
        public string RoomCode { get; set; }
        
        public int Capacity { get; set; }
        
        public int TheaterCapacity { get; set; }
        
        public int ClassroomCapacity { get; set; }
        
        public int UShapeCapacity { get; set; }
        
        public int BoardroomCapacity { get; set; }
        
        public decimal Area { get; set; } // Square meters
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal HalfDayPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal FullDayPrice { get; set; }
        
        public bool HasProjector { get; set; }
        
        public bool HasSoundSystem { get; set; }
        
        public bool HasWhiteboard { get; set; }
        
        public bool HasFlipChart { get; set; }
        
        public bool HasWiFi { get; set; }
        
        public bool HasAirConditioning { get; set; }
        
        public bool HasNaturalLight { get; set; }
        
        public bool HasBlackout { get; set; }
        
        public List<string> Images { get; set; }
        
        public string Floor { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}