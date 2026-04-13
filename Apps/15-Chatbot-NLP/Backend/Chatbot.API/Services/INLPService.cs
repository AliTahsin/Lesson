namespace Chatbot.API.Services
{
    public interface INLPService
    {
        Task<NLPResult> ProcessMessageAsync(string message);
        Task<string> DetectIntentAsync(string message);
        Task<Dictionary<string, string>> ExtractEntitiesAsync(string message, string intent);
    }

    public class NLPResult
    {
        public string Intent { get; set; }
        public Dictionary<string, string> Entities { get; set; }
        public string Response { get; set; }
        public decimal Confidence { get; set; }
    }
}