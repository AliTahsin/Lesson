namespace Auth.API.DTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }

    public class CreatePermissionDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Code { get; set; }
        
        [MaxLength(50)]
        public string Category { get; set; }
        
        [MaxLength(200)]
        public string Description { get; set; }
    }
}