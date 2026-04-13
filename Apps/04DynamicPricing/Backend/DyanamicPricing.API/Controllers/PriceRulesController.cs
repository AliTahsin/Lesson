using Microsoft.AspNetCore.Mvc;
using DynamicPricing.API.Services;

namespace DynamicPricing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceRulesController : ControllerBase
    {
        private readonly PricingEngineService _service;

        public PriceRulesController()
        {
            _service = new PricingEngineService();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAllPriceRules());
        }

        [HttpGet("seasons")]
        public IActionResult GetSeasons()
        {
            return Ok(_service.GetAllSeasons());
        }
    }
}