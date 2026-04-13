using Microsoft.AspNetCore.Mvc;
using DynamicPricing.API.Services;
using DynamicPricing.API.DTOs;

namespace DynamicPricing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricingController : ControllerBase
    {
        private readonly PricingEngineService _service;

        public PricingController()
        {
            _service = new PricingEngineService();
        }

        [HttpPost("calculate")]
        public IActionResult CalculatePrice([FromBody] PriceRequestDto request)
        {
            try
            {
                var result = _service.CalculatePrice(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("prices")]
        public IActionResult GetAllPrices()
        {
            return Ok(_service.GetAllDynamicPrices());
        }

        [HttpGet("prices/room/{roomId}")]
        public IActionResult GetPricesByRoom(int roomId, DateTime startDate, DateTime endDate)
        {
            return Ok(_service.GetDynamicPricesByRoom(roomId, startDate, endDate));
        }

        [HttpGet("demand/{hotelId}")]
        public IActionResult GetDemandFactors(int hotelId, DateTime startDate, DateTime endDate)
        {
            return Ok(_service.GetDemandFactors(hotelId, startDate, endDate));
        }

        [HttpGet("stats/{hotelId}")]
        public IActionResult GetStatistics(int hotelId, DateTime startDate, DateTime endDate)
        {
            return Ok(_service.GetPricingStatistics(hotelId, startDate, endDate));
        }

        [HttpPut("price")]
        public IActionResult UpdatePrice(int roomId, DateTime date, decimal price)
        {
            var result = _service.UpdateDynamicPrice(roomId, date, price);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}