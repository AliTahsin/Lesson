namespace CRM_Loyalty.API.DTOs
{
    public class PreferenceDto
    {
        public int Id { get; set; }
        public string PreferenceType { get; set; }
        public string PreferenceValue { get; set; }
        public string Description { get; set; }
    }
}
