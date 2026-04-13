using Chatbot.API.Repositories;

namespace Chatbot.API.Services
{
    public class NLPService : INLPService
    {
        private readonly IIntentRepository _intentRepository;

        // Turkish keywords for intent detection
        private readonly Dictionary<string, string[]> _intentKeywords = new()
        {
            { "BookRoom", new[] { "rezervasyon", "oda ayırt", "yer ayırt", "oda bul", "boş oda", "kayıt yap", "konaklama" } },
            { "CheckReservation", new[] { "rezervasyon sorgula", "rezervasyonumu gör", "rezervasyon kontrol", "rezervasyon nerede", "rezervasyon numarası" } },
            { "CancelReservation", new[] { "rezervasyon iptal", "iptal et", "vazgeçtim", "rezervasyonumu iptal et" } },
            { "HotelInfo", new[] { "otel bilgi", "otel hakkında", "otel özellikleri", "otel nerede", "konum", "ulaşım" } },
            { "RoomInfo", new[] { "oda bilgi", "oda tipi", "oda fiyat", "oda özellikleri", "oda büyüklüğü", "manzara" } },
            { "RestaurantInfo", new[] { "restoran", "yemek", "menü", "kahvaltı", "akşam yemeği", "rezervasyon restoran" } },
            { "Prices", new[] { "fiyat", "ücret", "ne kadar", "maliyet", "indirim", "kampanya" } },
            { "CheckInOut", new[] { "check-in", "check-out", "giriş saati", "çıkış saati", "erken giriş", "geç çıkış" } },
            { "Facilities", new[] { "havuz", "spa", "fitness", "otopark", "wifi", "restoran", "bar" } },
            { "Complaint", new[] { "şikayet", "sorun", "problem", "rahatsız", "memnun değil", "yardım" } },
            { "ThankYou", new[] { "teşekkür", "sağ ol", "eyvallah", "thanks" } },
            { "Greeting", new[] { "merhaba", "selam", "iyi günler", "günaydın", "iyi akşamlar", "hello", "hi" } }
        };

        // Response templates
        private readonly Dictionary<string, string[]> _responses = new()
        {
            { "BookRoom", new[] { 
                "Rezervasyon yapmak için lütfen otel adı, giriş ve çıkış tarihlerinizi belirtir misiniz?",
                "Tabii, size rezervasyon konusunda yardımcı olabilirim. Hangi otel için ve hangi tarihlerde konaklamak istiyorsunuz?"
            } },
            { "CheckReservation", new[] {
                "Rezervasyonunuzu sorgulamak için lütfen rezervasyon numaranızı veya e-posta adresinizi paylaşır mısınız?"
            } },
            { "CancelReservation", new[] {
                "Rezervasyon iptali için lütfen rezervasyon numaranızı belirtir misiniz?"
            } },
            { "HotelInfo", new[] {
                "Otelimiz şehir merkezinde yer almaktadır. 250 odamız, spa merkezimiz, kapalı ve açık havuzlarımız bulunmaktadır. Daha detaylı bilgi ister misiniz?"
            } },
            { "RoomInfo", new[] {
                "Oda tiplerimiz: Standart, Deluxe, Suite ve Presidential. Tüm odalarımızda klima, mini bar, uydu TV ve ücretsiz WiFi bulunmaktadır. Hangi oda tipi hakkında bilgi almak istersiniz?"
            } },
            { "RestaurantInfo", new[] {
                "Otelimizde 3 farklı restoranımız bulunmaktadır: Ana Restoran (uluslararası mutfak), İtalyan Restoran ve Sushi Bar. Rezervasyon yaptırmak ister misiniz?"
            } },
            { "Prices", new[] {
                "Oda fiyatlarımız sezona göre değişiklik göstermektedir. Giriş tarihinizi belirtirseniz size özel fiyat teklifi sunabiliriz."
            } },
            { "CheckInOut", new[] {
                "Check-in saati 14:00, check-out saati 12:00'dir. Erken giriş veya geç çıkış için talebinizi iletebilirsiniz."
            } },
            { "Facilities", new[] {
                "Otelimizde: Açık ve kapalı yüzme havuzu, spa merkezi (sauna, buhar odası, masaj), fitness center, çocuk kulübü, 3 restoran ve 2 bar bulunmaktadır."
            } },
            { "Complaint", new[] {
                "Yaşadığınız sorun için üzgünüz. Lütfen detayları belirtir misiniz, size en kısa sürede yardımcı olalım."
            } },
            { "ThankYou", new[] {
                "Rica ederim! Başka bir konuda yardımcı olabilirsem lütfen yazın. İyi günler dilerim!",
                "Ne demek! Size yardımcı olabildiğime sevindim. İyi günler!"
            } },
            { "Greeting", new[] {
                "Merhaba! Hoş geldiniz. Size nasıl yardımcı olabilirim?",
                "İyi günler! Otelimizle ilgili tüm sorularınızı cevaplamaktan mutluluk duyarım."
            } }
        };

        public NLPService(IIntentRepository intentRepository)
        {
            _intentRepository = intentRepository;
        }

        public async Task<NLPResult> ProcessMessageAsync(string message)
        {
            var lowerMessage = message.ToLower();
            var intent = await DetectIntentAsync(lowerMessage);
            var entities = await ExtractEntitiesAsync(lowerMessage, intent);
            var response = GenerateResponse(intent);
            
            return new NLPResult
            {
                Intent = intent,
                Entities = entities,
                Response = response,
                Confidence = 0.85m
            };
        }

        public async Task<string> DetectIntentAsync(string message)
        {
            var bestMatch = "General";
            var bestScore = 0;
            
            foreach (var intent in _intentKeywords)
            {
                var score = intent.Value.Count(keyword => message.Contains(keyword));
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMatch = intent.Key;
                }
            }
            
            return await Task.FromResult(bestMatch);
        }

        public async Task<Dictionary<string, string>> ExtractEntitiesAsync(string message, string intent)
        {
            var entities = new Dictionary<string, string>();
            
            // Extract date patterns
            var datePattern = @"(\d{1,2}[/.-]\d{1,2}[/.-]\d{2,4})|(\d{1,2}\s+(Ocak|Şubat|Mart|Nisan|Mayıs|Haziran|Temmuz|Ağustos|Eylül|Ekim|Kasım|Aralık)\s+\d{2,4})";
            var dateMatch = System.Text.RegularExpressions.Regex.Match(message, datePattern);
            if (dateMatch.Success)
            {
                entities["Date"] = dateMatch.Value;
            }
            
            // Extract number patterns (room count, person count)
            var numberPattern = @"\d+";
            var numberMatches = System.Text.RegularExpressions.Regex.Matches(message, numberPattern);
            if (numberMatches.Count > 0)
            {
                entities["Number"] = numberMatches[0].Value;
            }
            
            return await Task.FromResult(entities);
        }

        private string GenerateResponse(string intent)
        {
            if (_responses.ContainsKey(intent))
            {
                var responses = _responses[intent];
                var random = new Random();
                return responses[random.Next(responses.Length)];
            }
            
            return "Üzgünüm, sorunuzu tam anlayamadım. Lütfen biraz daha açıklayıcı olur musunuz?";
        }
    }
}