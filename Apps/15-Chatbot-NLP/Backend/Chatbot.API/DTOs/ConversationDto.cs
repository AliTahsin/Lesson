namespace Chatbot.API.DTOs
{
    public class ConversationDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int? HotelId { get; set; }
        public string Status { get; set; }
        public int? AssignedAgentId { get; set; }
        public string AssignedAgentName { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public int MessageCount { get; set; }
        public bool IsBotActive { get; set; }
    }

    public class ChatStatisticsDto
    {
        public int TotalConversations { get; set; }
        public int ActiveConversations { get; set; }
        public int ResolvedConversations { get; set; }
        public int AverageResponseTime { get; set; }
        public decimal SatisfactionRate { get; set; }
    }
}