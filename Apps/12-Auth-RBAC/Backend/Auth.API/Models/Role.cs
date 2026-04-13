using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(200)]
        public string Description { get; set; }
        
        [MaxLength(20)]
        public string Level { get; set; } // SuperAdmin, Admin, Manager, Staff, Guest
        
        public List<int> PermissionIds { get; set; }
        
        public bool IsDefault { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}