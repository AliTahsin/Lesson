namespace Chatbot.API.DTOs
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string SenderType { get; set; }
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
        public string Intent { get; set; }
        public string Status { get; set; }
        public DateTime SentAt { get; set; }
    }

    public class SendMessageDto
    {
        public int ConversationId { get; set; }
        public string Message { get; set; }
    }
}