namespace CRM_Loyalty.API.Models
{
    public class CustomerPreference
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string PreferenceType { get; set; } // Room, Floor, Bed, Pillow, Dietary, etc.
        public string PreferenceValue { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}