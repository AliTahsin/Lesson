namespace MobileCustomer.API.Services
{
    public interface ILanguageService
    {
        Task<List<LanguageDto>> GetSupportedLanguagesAsync();
        Task<string> GetUserLanguageAsync(int userId);
        Task<bool> SetUserLanguageAsync(int userId, string languageCode);
        Task<Dictionary<string, string>> GetTranslationsAsync(string languageCode);
    }

    public class LanguageDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NativeName { get; set; }
        public bool IsRTL { get; set; }
    }
}