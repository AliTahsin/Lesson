namespace MobileCustomer.API.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _translations;

        public LanguageService()
        {
            _translations = new Dictionary<string, Dictionary<string, string>>
            {
                ["tr"] = new Dictionary<string, string>
                {
                    { "welcome", "Hoş Geldiniz" },
                    { "hotel_name", "Otel Yönetim Sistemi" },
                    { "rooms", "Odalar" },
                    { "reservations", "Rezervasyonlar" },
                    { "profile", "Profil" },
                    { "settings", "Ayarlar" },
                    { "logout", "Çıkış Yap" },
                    { "login", "Giriş Yap" },
                    { "register", "Kayıt Ol" },
                    { "search", "Ara" },
                    { "check_in", "Giriş Tarihi" },
                    { "check_out", "Çıkış Tarihi" },
                    { "guests", "Misafir Sayısı" },
                    { "book_now", "Şimdi Rezervasyon Yap" },
                    { "total_price", "Toplam Fiyat" },
                    { "payment", "Ödeme" },
                    { "digital_key", "Dijital Anahtar" },
                    { "room_service", "Oda Servisi" },
                    { "spa", "Spa Merkezi" },
                    { "notifications", "Bildirimler" }
                },
                ["en"] = new Dictionary<string, string>
                {
                    { "welcome", "Welcome" },
                    { "hotel_name", "Hotel Management System" },
                    { "rooms", "Rooms" },
                    { "reservations", "Reservations" },
                    { "profile", "Profile" },
                    { "settings", "Settings" },
                    { "logout", "Logout" },
                    { "login", "Login" },
                    { "register", "Register" },
                    { "search", "Search" },
                    { "check_in", "Check-in Date" },
                    { "check_out", "Check-out Date" },
                    { "guests", "Number of Guests" },
                    { "book_now", "Book Now" },
                    { "total_price", "Total Price" },
                    { "payment", "Payment" },
                    { "digital_key", "Digital Key" },
                    { "room_service", "Room Service" },
                    { "spa", "Spa Center" },
                    { "notifications", "Notifications" }
                },
                ["de"] = new Dictionary<string, string>
                {
                    { "welcome", "Willkommen" },
                    { "hotel_name", "Hotel-Management-System" },
                    { "rooms", "Zimmer" },
                    { "reservations", "Reservierungen" },
                    { "profile", "Profil" },
                    { "settings", "Einstellungen" },
                    { "logout", "Ausloggen" },
                    { "login", "Anmelden" },
                    { "register", "Registrieren" },
                    { "search", "Suchen" },
                    { "check_in", "Check-in Datum" },
                    { "check_out", "Check-out Datum" },
                    { "guests", "Anzahl der Gäste" },
                    { "book_now", "Jetzt Buchen" },
                    { "total_price", "Gesamtpreis" },
                    { "payment", "Zahlung" },
                    { "digital_key", "Digitaler Schlüssel" },
                    { "room_service", "Zimmerservice" },
                    { "spa", "Spa-Zentrum" },
                    { "notifications", "Benachrichtigungen" }
                },
                ["ru"] = new Dictionary<string, string>
                {
                    { "welcome", "Добро пожаловать" },
                    { "hotel_name", "Система управления отелем" },
                    { "rooms", "Номера" },
                    { "reservations", "Бронирования" },
                    { "profile", "Профиль" },
                    { "settings", "Настройки" },
                    { "logout", "Выйти" },
                    { "login", "Войти" },
                    { "register", "Зарегистрироваться" },
                    { "search", "Поиск" },
                    { "check_in", "Дата заезда" },
                    { "check_out", "Дата выезда" },
                    { "guests", "Количество гостей" },
                    { "book_now", "Забронировать сейчас" },
                    { "total_price", "Общая стоимость" },
                    { "payment", "Оплата" },
                    { "digital_key", "Цифровой ключ" },
                    { "room_service", "Обслуживание номеров" },
                    { "spa", "Спа-центр" },
                    { "notifications", "Уведомления" }
                }
            };
        }

        public async Task<List<LanguageDto>> GetSupportedLanguagesAsync()
        {
            var languages = new List<LanguageDto>
            {
                new LanguageDto { Code = "tr", Name = "Turkish", NativeName = "Türkçe", IsRTL = false },
                new LanguageDto { Code = "en", Name = "English", NativeName = "English", IsRTL = false },
                new LanguageDto { Code = "de", Name = "German", NativeName = "Deutsch", IsRTL = false },
                new LanguageDto { Code = "ru", Name = "Russian", NativeName = "Русский", IsRTL = false }
            };
            
            return await Task.FromResult(languages);
        }

        public async Task<string> GetUserLanguageAsync(int userId)
        {
            // Mock: return user's preferred language
            return await Task.FromResult("tr");
        }

        public async Task<bool> SetUserLanguageAsync(int userId, string languageCode)
        {
            // Mock: save user's language preference
            return await Task.FromResult(true);
        }

        public async Task<Dictionary<string, string>> GetTranslationsAsync(string languageCode)
        {
            if (_translations.ContainsKey(languageCode))
                return await Task.FromResult(_translations[languageCode]);
            
            return await Task.FromResult(_translations["en"]);
        }
    }
}