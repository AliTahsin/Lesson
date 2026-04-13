using HotelManagement.API.Models;
using System.Collections.Generic;

namespace HotelManagement.API.Data
{
    public static class MockData
    {
        public static List<Chain> GetChains()
        {
            return new List<Chain>
            {
                new Chain { Id = 1, Name = "Marriott International", Headquarters = "Bethesda, Maryland, USA", Founder = "J. Willard Marriott", FoundedYear = 1927, TotalHotels = 8200, Website = "marriott.com", LogoUrl = "marriott_logo.png", Description = "Global hospitality leader" },
                new Chain { Id = 2, Name = "Hilton Worldwide", Headquarters = "McLean, Virginia, USA", Founder = "Conrad Hilton", FoundedYear = 1919, TotalHotels = 7100, Website = "hilton.com", LogoUrl = "hilton_logo.png", Description = "Leading global hospitality company" },
                new Chain { Id = 3, Name = "Accor", Headquarters = "Paris, France", Founder = "Paul Dubrule", FoundedYear = 1967, TotalHotels = 5300, Website = "accor.com", LogoUrl = "accor_logo.png", Description = "World-leading augmented hospitality group" },
                new Chain { Id = 4, Name = "InterContinental Hotels Group", Headquarters = "Denham, UK", Founder = "Juan Trippe", FoundedYear = 1946, TotalHotels = 6000, Website = "ihg.com", LogoUrl = "ihg_logo.png", Description = "Global hotel company" }
            };
        }

        public static List<Brand> GetBrands()
        {
            return new List<Brand>
            {
                new Brand { Id = 1, Name = "The Ritz-Carlton", ChainId = 1, Segment = "Luxury", LogoUrl = "ritz.png", Description = "Ultimate luxury", MinStarRating = 5, MaxStarRating = 5 },
                new Brand { Id = 2, Name = "JW Marriott", ChainId = 1, Segment = "Luxury", LogoUrl = "jw.png", Description = "Luxury brand", MinStarRating = 5, MaxStarRating = 5 },
                new Brand { Id = 3, Name = "Marriott", ChainId = 1, Segment = "Upscale", LogoUrl = "marriott.png", Description = "Full-service hotels", MinStarRating = 4, MaxStarRating = 5 },
                new Brand { Id = 4, Name = "Sheraton", ChainId = 1, Segment = "Upscale", LogoUrl = "sheraton.png", Description = "Classic comfort", MinStarRating = 4, MaxStarRating = 5 },
                new Brand { Id = 5, Name = "Waldorf Astoria", ChainId = 2, Segment = "Luxury", LogoUrl = "waldorf.png", Description = "Iconic luxury", MinStarRating = 5, MaxStarRating = 5 },
                new Brand { Id = 6, Name = "Hilton", ChainId = 2, Segment = "Upscale", LogoUrl = "hilton.png", Description = "Full-service", MinStarRating = 4, MaxStarRating = 5 },
                new Brand { Id = 7, Name = "DoubleTree", ChainId = 2, Segment = "Upscale", LogoUrl = "doubletree.png", Description = "Upper upscale", MinStarRating = 4, MaxStarRating = 4 },
                new Brand { Id = 8, Name = "Sofitel", ChainId = 3, Segment = "Luxury", LogoUrl = "sofitel.png", Description = "French luxury", MinStarRating = 5, MaxStarRating = 5 },
                new Brand { Id = 9, Name = "Novotel", ChainId = 3, Segment = "Midscale", LogoUrl = "novotel.png", Description = "Midscale", MinStarRating = 3, MaxStarRating = 4 },
                new Brand { Id = 10, Name = "InterContinental", ChainId = 4, Segment = "Luxury", LogoUrl = "intercontinental.png", Description = "Global luxury", MinStarRating = 5, MaxStarRating = 5 }
            };
        }

        public static List<Hotel> GetHotels()
        {
            return new List<Hotel>
            {
                new Hotel 
                { 
                    Id = 1, Name = "The Ritz-Carlton Istanbul", BrandId = 1, City = "İstanbul", Country = "Turkey",
                    Address = "Suzer Plaza, Askerocagi Caddesi, No:15", StarRating = 5, Phone = "+90 212 334 4444",
                    Email = "info@ritzcarltonistanbul.com", Website = "ritzcarlton.com/istanbul",
                    Description = "Luxury hotel with Bosphorus view", TotalRooms = 250,
                    OpeningDate = new DateTime(2010, 5, 15), Status = "Active",
                    Amenities = new List<string> { "Spa", "Pool", "Restaurant", "Bar", "Fitness", "Conference" },
                    Images = new List<string> { "hotel1_1.jpg", "hotel1_2.jpg" },
                    Location = new GeoLocation { Latitude = 41.0379, Longitude = 28.9855 }
                },
                new Hotel 
                { 
                    Id = 2, Name = "JW Marriott Ankara", BrandId = 2, City = "Ankara", Country = "Turkey",
                    Address = "Kızılırmak Mah., Muhsin Yazıcıoğlu Cad.", StarRating = 5, Phone = "+90 312 123 4567",
                    Email = "info@jwmarriottankara.com", Website = "jwmarriott.com/ankara",
                    Description = "Business luxury hotel", TotalRooms = 300,
                    OpeningDate = new DateTime(2015, 8, 20), Status = "Active",
                    Amenities = new List<string> { "Spa", "Pool", "Restaurant", "Business Center" },
                    Images = new List<string> { "hotel2_1.jpg", "hotel2_2.jpg" },
                    Location = new GeoLocation { Latitude = 39.9334, Longitude = 32.8597 }
                },
                new Hotel 
                { 
                    Id = 3, Name = "Hilton Istanbul Bosphorus", BrandId = 6, City = "İstanbul", Country = "Turkey",
                    Address = "Cumhuriyet Cad., Harbiye", StarRating = 5, Phone = "+90 212 315 6000",
                    Email = "info@hiltonistanbul.com", Website = "hilton.com/istanbul",
                    Description = "Iconic hotel with Bosphorus view", TotalRooms = 500,
                    OpeningDate = new DateTime(2000, 1, 1), Status = "Active",
                    Amenities = new List<string> { "Pool", "Tennis", "Spa", "Multiple Restaurants" },
                    Images = new List<string> { "hotel3_1.jpg", "hotel3_2.jpg" },
                    Location = new GeoLocation { Latitude = 41.0499, Longitude = 28.9941 }
                },
                new Hotel 
                { 
                    Id = 4, Name = "DoubleTree by Hilton Izmir", BrandId = 7, City = "İzmir", Country = "Turkey",
                    Address = "Gaziosmanpaşa Bulvarı, No:24", StarRating = 4, Phone = "+90 232 456 7890",
                    Email = "info@doubletreeizmir.com", Website = "doubletree.com/izmir",
                    Description = "Modern comfort", TotalRooms = 180,
                    OpeningDate = new DateTime(2018, 3, 10), Status = "Active",
                    Amenities = new List<string> { "Pool", "Restaurant", "Bar" },
                    Images = new List<string> { "hotel4_1.jpg" },
                    Location = new GeoLocation { Latitude = 38.4192, Longitude = 27.1287 }
                },
                new Hotel 
                { 
                    Id = 5, Name = "Sofitel Bodrum", BrandId = 8, City = "Bodrum", Country = "Turkey",
                    Address = "Turgutreis Mah., Plaj Cad.", StarRating = 5, Phone = "+90 252 345 6789",
                    Email = "info@sofitelbodrum.com", Website = "sofitel.com/bodrum",
                    Description = "French luxury on Aegean", TotalRooms = 150,
                    OpeningDate = new DateTime(2019, 6, 1), Status = "Active",
                    Amenities = new List<string> { "Private Beach", "Spa", "Infinity Pool" },
                    Images = new List<string> { "hotel5_1.jpg", "hotel5_2.jpg" },
                    Location = new GeoLocation { Latitude = 37.0254, Longitude = 27.2234 }
                },
                new Hotel 
                { 
                    Id = 6, Name = "Novotel Antalya", BrandId = 9, City = "Antalya", Country = "Turkey",
                    Address = "Lara Cad., No:123", StarRating = 4, Phone = "+90 242 234 5678",
                    Email = "info@novotelantalya.com", Website = "novotel.com/antalya",
                    Description = "Family friendly", TotalRooms = 220,
                    OpeningDate = new DateTime(2016, 4, 20), Status = "Active",
                    Amenities = new List<string> { "Pool", "Kids Club", "Restaurant" },
                    Images = new List<string> { "hotel6_1.jpg" },
                    Location = new GeoLocation { Latitude = 36.8969, Longitude = 30.7133 }
                }
            };
        }

        public static List<Hotel> GetHotelsByBrand(int brandId)
        {
            return GetHotels().Where(h => h.BrandId == brandId).ToList();
        }

        public static List<Hotel> GetHotelsByChain(int chainId)
        {
            var brandIds = GetBrands().Where(b => b.ChainId == chainId).Select(b => b.Id).ToList();
            return GetHotels().Where(h => brandIds.Contains(h.BrandId)).ToList();
        }
    }
}