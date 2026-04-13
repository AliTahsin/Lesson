namespace ChannelManagement.API.DTOs
{
    public class ChannelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public decimal Commission { get; set; }
        public decimal Markup { get; set; }
        public bool IsActive { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
    }

    public class ChannelDetailDto : ChannelDto
    {
        public string ApiEndpoint { get; set; }
        public int SyncIntervalMinutes { get; set; }
        public DateTime LastSyncDate { get; set; }
        public List<string> SupportedCurrencies { get; set; }
        public List<string> SupportedLanguages { get; set; }
    }

    public class CreateChannelDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string ApiEndpoint { get; set; }
        public decimal Commission { get; set; }
        public decimal Markup { get; set; }
        public string Description { get; set; }
    }
}