using Microsoft.AspNetCore.Mvc;
using HotelManagement.API.Services;

namespace HotelManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChainsController : ControllerBase
    {
        private readonly HotelManagementService _service;

        public ChainsController()
        {
            _service = new HotelManagementService();
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAllChains());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var chain = _service.GetChainById(id);
            if (chain == null) return NotFound();
            return Ok(chain);
        }
    }
}