using Chatbot.API.Models;

namespace Chatbot.API.Data
{
    public static class MockData
    {
        private static readonly Random _random = new();

        public static List<Conversation> GetConversations()
        {
            var conversations = new List<Conversation>();
            
            for (int i = 1; i <= 50; i++)
            {
                var isActive = i <= 5;
                var startDate = DateTime.Now.AddDays(-_random.Next(0, 30));
                
                conversations.Add(new Conversation
                {
                    Id = i,
                    UserId = _random.Next(1, 20),
                    UserName = $"User {_random.Next(1, 20)}",
                    UserEmail = $"user{_random.Next(1, 20)}@email.com",
                    HotelId = _random.Next(1, 4),
                    Status = isActive ? "Active" : "Resolved",
                    AssignedAgentId = isActive ? _random.Next(1, 5) : null,
                    AssignedAgentName = isActive ? $"Agent {_random.Next(1, 5)}" : null,
                    StartedAt = startDate,
                    EndedAt = isActive ? null : startDate.AddMinutes(_random.Next(5, 60)),
                    LastMessageAt = startDate.AddMinutes(_random.Next(1, 30)),
                    MessageCount = _random.Next(3, 20),
                    IsBotActive = true,
                    CreatedAt = startDate
                });
            }
            
            return conversations;
        }

        public static List<ChatMessage> GetMessages()
        {
            var messages = new List<ChatMessage>();
            var conversations = GetConversations();
            
            foreach (var conversation in conversations)
            {
                var messageCount = conversation.MessageCount;
                for (int i = 1; i <= messageCount; i++)
                {
                    var isUser = i % 2 == 1;
                    var sentAt = conversation.StartedAt.AddMinutes(i * 2);
                    
                    messages.Add(new ChatMessage
                    {
                        Id = messages.Count + 1,
                        ConversationId = conversation.Id,
                        SenderType = isUser ? "User" : "Bot",
                        SenderId = isUser ? conversation.UserId : null,
                        SenderName = isUser ? conversation.UserName : "Hotel Assistant",
                        Message = isUser ? GetUserMessage(i) : GetBotResponse(i),
                        Intent = isUser ? null : GetIntentForMessage(i),
                        Status = "Read",
                        SentAt = sentAt,
                        ReadAt = sentAt.AddSeconds(5)
                    });
                }
            }
            
            return messages;
        }

        public static List<Intent> GetIntents()
        {
            return new List<Intent>
            {
                new Intent
                {
                    Id = 1,
                    Name = "BookRoom",
                    Description = "User wants to book a room",
                    TrainingPhrases = new List<string> { "oda ayırt", "rezervasyon yap", "yer ayırt", "oda bul", "boş oda" },
                    Responses = new List<string> { 
                        "Rezervasyon yapmak için lütfen tarih ve oda tipi belirtir misiniz?",
                        "Tabii, size rezervasyon konusunda yardımcı olabilirim. Hangi tarihler için bakıyorsunuz?"
                    },
                    Action = "BookRoom",
                    ConfidenceThreshold = 0.7m,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new Intent
                {
                    Id = 2,
                    Name = "HotelInfo",
                    Description = "User asks about hotel information",
                    TrainingPhrases = new List<string> { "otel bilgi", "otel hakkında", "otel özellikleri", "oteli tanı" },
                    Responses = new List<string> { 
                        "Otelimiz şehir merkezinde yer almaktadır. 250 odamız, spa merkezimiz, kapalı ve açık havuzlarımız bulunmaktadır."
                    },
                    Action = "GetInfo",
                    ConfidenceThreshold = 0.7m,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new Intent
                {
                    Id = 3,
                    Name = "Prices",
                    Description = "User asks about prices",
                    TrainingPhrases = new List<string> { "fiyat", "ücret", "ne kadar", "maliyet", "kaç para" },
                    Responses = new List<string> { 
                        "Oda fiyatlarımız sezona göre değişmektedir. Hangi tarihler için bakıyorsunuz?"
                    },
                    Action = "GetPrice",
                    ConfidenceThreshold = 0.7m,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new Intent
                {
                    Id = 4,
                    Name = "Facilities",
                    Description = "User asks about hotel facilities",
                    TrainingPhrases = new List<string> { "havuz", "spa", "fitness", "otopark", "wifi", "restoran" },
                    Responses = new List<string> { 
                        "Otelimizde: Açık ve kapalı yüzme havuzu, spa merkezi, fitness center, çocuk kulübü, 3 restoran ve 2 bar bulunmaktadır."
                    },
                    Action = "GetFacilities",
                    ConfidenceThreshold = 0.7m,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new Intent
                {
                    Id = 5,
                    Name = "Greeting",
                    Description = "User greets the bot",
                    TrainingPhrases = new List<string> { "merhaba", "selam", "iyi günler", "günaydın", "hello" },
                    Responses = new List<string> { 
                        "Merhaba! Hoş geldiniz. Size nasıl yardımcı olabilirim?",
                        "İyi günler! Otelimizle ilgili tüm sorularınızı cevaplamaktan mutluluk duyarım."
                    },
                    Action = "Greet",
                    ConfidenceThreshold = 0.8m,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                }
            };
        }

        private static string GetUserMessage(int index)
        {
            var messages = new[]
            {
                "Merhaba, oteliniz hakkında bilgi alabilir miyim?",
                "Oda fiyatları nedir?",
                "Havuzunuz var mı?",
                "Rezervasyon yapmak istiyorum",
                "Teşekkürler, iyi günler"
            };
            return messages[index % messages.Length];
        }

        private static string GetBotResponse(int index)
        {
            var responses = new[]
            {
                "Merhaba! Hoş geldiniz. Size nasıl yardımcı olabilirim?",
                "Oda fiyatlarımız sezona göre değişmektedir. Hangi tarihler için bakıyorsunuz?",
                "Evet, otelimizde açık ve kapalı yüzme havuzumuz bulunmaktadır.",
                "Tabii, rezervasyon için size yardımcı olabilirim. Lütfen giriş ve çıkış tarihlerinizi belirtir misiniz?",
                "Rica ederim! Başka bir konuda yardımcı olabilirsem lütfen yazın."
            };
            return responses[index % responses.Length];
        }

        private static string GetIntentForMessage(int index)
        {
            var intents = new[] { "Greeting", "Prices", "Facilities", "BookRoom", "ThankYou" };
            return intents[index % intents.Length];
        }
    }
}