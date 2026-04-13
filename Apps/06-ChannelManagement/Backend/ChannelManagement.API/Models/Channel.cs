namespace ChannelManagement.API.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; } // OTA, GDS, Direct, Meta
        public string ApiEndpoint { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public decimal Commission { get; set; }
        public decimal Markup { get; set; }
        public bool IsActive { get; set; }
        public int SyncIntervalMinutes { get; set; }
        public DateTime LastSyncDate { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public List<string> SupportedCurrencies { get; set; }
        public List<string> SupportedLanguages { get; set; }
    }
}