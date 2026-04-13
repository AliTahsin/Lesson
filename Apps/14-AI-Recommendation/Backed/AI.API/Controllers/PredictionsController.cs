using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AI.API.DTOs;
using AI.API.Services;

namespace AI.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionsController : ControllerBase
    {
        private readonly IPredictionService _predictionService;

        public PredictionsController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpGet("demand")]
        public async Task<IActionResult> PredictDemand([FromQuery] int hotelId, [FromQuery] DateTime date)
        {
            var prediction = await _predictionService.PredictDemandAsync(hotelId, date);
            return Ok(prediction);
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> PredictRevenue([FromQuery] int hotelId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var prediction = await _predictionService.PredictRevenueAsync(hotelId, startDate, endDate);
            return Ok(prediction);
        }

        [HttpGet("occupancy")]
        public async Task<IActionResult> PredictOccupancy([FromQuery] int hotelId, [FromQuery] DateTime date)
        {
            var prediction = await _predictionService.PredictOccupancyAsync(hotelId, date);
            return Ok(prediction);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetPredictionHistory([FromQuery] int hotelId, [FromQuery] string predictionType)
        {
            var history = await _predictionService.GetPredictionHistoryAsync(hotelId, predictionType);
            return Ok(history);
        }
    }
}