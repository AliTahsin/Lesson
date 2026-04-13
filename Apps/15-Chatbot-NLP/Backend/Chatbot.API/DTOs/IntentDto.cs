namespace Chatbot.API.DTOs
{
    public class IntentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> TrainingPhrases { get; set; }
        public List<string> Responses { get; set; }
        public string Action { get; set; }
        public decimal ConfidenceThreshold { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateIntentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> TrainingPhrases { get; set; }
        public List<string> Responses { get; set; }
        public string Action { get; set; }
        public decimal ConfidenceThreshold { get; set; }
    }

    public class UpdateIntentDto
    {
        public string Description { get; set; }
        public List<string> TrainingPhrases { get; set; }
        public List<string> Responses { get; set; }
        public string Action { get; set; }
        public decimal ConfidenceThreshold { get; set; }
        public bool IsActive { get; set; }
    }
}