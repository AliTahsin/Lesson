using Microsoft.AspNetCore.Mvc;
using HotelManagement.API.Services;
using HotelManagement.API.DTOs;
using HotelManagement.API.Models;

namespace HotelManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly HotelManagementService _service;

        public HotelsController()
        {
            _service = new HotelManagementService();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var hotels = _service.GetAllHotels();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var hotel = _service.GetHotelById(id);
            if (hotel == null)
                return NotFound($"Hotel with id {id} not found");
            return Ok(hotel);
        }

        [HttpGet("brand/{brandId}")]
        public IActionResult GetByBrand(int brandId)
        {
            return Ok(_service.GetHotelsByBrand(brandId));
        }

        [HttpGet("chain/{chainId}")]
        public IActionResult GetByChain(int chainId)
        {
            return Ok(_service.GetHotelsByChain(chainId));
        }

        [HttpGet("city/{city}")]
        public IActionResult GetByCity(string city)
        {
            return Ok(_service.GetHotelsByCity(city));
        }

        [HttpGet("country/{country}")]
        public IActionResult GetByCountry(string country)
        {
            return Ok(_service.GetHotelsByCountry(country));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            return Ok(_service.SearchHotels(keyword));
        }

        [HttpGet("stats")]
        public IActionResult GetStatistics()
        {
            return Ok(_service.GetStatistics());
        }

        [HttpPost]
        public IActionResult Create([FromBody] Hotel hotel)
        {
            var created = _service.AddHotel(hotel);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Hotel hotel)
        {
            if (_service.UpdateHotel(id, hotel))
                return Ok(new { message = "Hotel updated successfully" });
            return NotFound($"Hotel with id {id} not found");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_service.DeleteHotel(id))
                return Ok(new { message = "Hotel deleted successfully" });
            return NotFound($"Hotel with id {id} not found");
        }
    }
}