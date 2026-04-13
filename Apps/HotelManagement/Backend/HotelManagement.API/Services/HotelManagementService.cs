using HotelManagement.API.Models;
using HotelManagement.API.DTOs;
using HotelManagement.API.Data;

namespace HotelManagement.API.Services
{
    public class HotelManagementService
    {
        private readonly List<Hotel> _hotels;
        private readonly List<Brand> _brands;
        private readonly List<Chain> _chains;

        public HotelManagementService()
        {
            _hotels = MockData.GetHotels();
            _brands = MockData.GetBrands();
            _chains = MockData.GetChains();
        }

        // Hotel Operations
        public List<Hotel> GetAllHotels() => _hotels;
        
        public Hotel GetHotelById(int id) => _hotels.FirstOrDefault(h => h.Id == id);
        
        public List<Hotel> GetHotelsByBrand(int brandId) => _hotels.Where(h => h.BrandId == brandId).ToList();
        
        public List<Hotel> GetHotelsByChain(int chainId)
        {
            var brandIds = _brands.Where(b => b.ChainId == chainId).Select(b => b.Id);
            return _hotels.Where(h => brandIds.Contains(h.BrandId)).ToList();
        }
        
        public List<Hotel> GetHotelsByCity(string city) => _hotels.Where(h => h.City.ToLower() == city.ToLower()).ToList();
        
        public List<Hotel> GetHotelsByCountry(string country) => _hotels.Where(h => h.Country.ToLower() == country.ToLower()).ToList();
        
        public List<Hotel> GetHotelsByStarRating(int minStar, int maxStar) => _hotels.Where(h => h.StarRating >= minStar && h.StarRating <= maxStar).ToList();
        
        public List<Hotel> SearchHotels(string keyword)
        {
            return _hotels.Where(h => 
                h.Name.ToLower().Contains(keyword.ToLower()) ||
                h.City.ToLower().Contains(keyword.ToLower()) ||
                h.Description.ToLower().Contains(keyword.ToLower())
            ).ToList();
        }

        public Hotel AddHotel(Hotel hotel)
        {
            hotel.Id = _hotels.Max(h => h.Id) + 1;
            _hotels.Add(hotel);
            return hotel;
        }

        public bool UpdateHotel(int id, Hotel updatedHotel)
        {
            var hotel = GetHotelById(id);
            if (hotel == null) return false;
            
            hotel.Name = updatedHotel.Name;
            hotel.BrandId = updatedHotel.BrandId;
            hotel.City = updatedHotel.City;
            hotel.Country = updatedHotel.Country;
            hotel.Address = updatedHotel.Address;
            hotel.StarRating = updatedHotel.StarRating;
            hotel.Phone = updatedHotel.Phone;
            hotel.Email = updatedHotel.Email;
            hotel.Description = updatedHotel.Description;
            hotel.Status = updatedHotel.Status;
            
            return true;
        }

        public bool DeleteHotel(int id)
        {
            var hotel = GetHotelById(id);
            if (hotel == null) return false;
            return _hotels.Remove(hotel);
        }

        // Brand Operations
        public List<Brand> GetAllBrands() => _brands;
        public Brand GetBrandById(int id) => _brands.FirstOrDefault(b => b.Id == id);
        public List<Brand> GetBrandsByChain(int chainId) => _brands.Where(b => b.ChainId == chainId).ToList();
        
        // Chain Operations
        public List<Chain> GetAllChains() => _chains;
        public Chain GetChainById(int id) => _chains.FirstOrDefault(c => c.Id == id);
        
        // Statistics
        public object GetStatistics()
        {
            return new
            {
                TotalHotels = _hotels.Count,
                TotalBrands = _brands.Count,
                TotalChains = _chains.Count,
                HotelsByCountry = _hotels.GroupBy(h => h.Country).Select(g => new { Country = g.Key, Count = g.Count() }),
                HotelsByStarRating = _hotels.GroupBy(h => h.StarRating).Select(g => new { StarRating = g.Key, Count = g.Count() })
            };
        }
    }
}