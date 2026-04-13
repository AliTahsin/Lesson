using Microsoft.AspNetCore.Mvc;
using HotelManagement.API.Services;

namespace HotelManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly HotelManagementService _service;

        public BrandsController()
        {
            _service = new HotelManagementService();
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAllBrands());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var brand = _service.GetBrandById(id);
            if (brand == null) return NotFound();
            return Ok(brand);
        }

        [HttpGet("chain/{chainId}")]
        public IActionResult GetByChain(int chainId) => Ok(_service.GetBrandsByChain(chainId));
    }
}