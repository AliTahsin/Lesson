using Microsoft.AspNetCore.Mvc;
using CRM_Loyalty.API.Services;
using CRM_Loyalty.API.DTOs;

namespace CRM_Loyalty.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoyaltyController : ControllerBase
    {
        private readonly LoyaltyService _service;

        public LoyaltyController()
        {
            _service = new LoyaltyService();
        }

        [HttpGet("info/{customerId}")]
        public IActionResult GetLoyaltyInfo(int customerId)
        {
            var info = _service.GetLoyaltyInfo(customerId);
            if (info == null) return NotFound();
            return Ok(info);
        }

        [HttpGet("history/{customerId}")]
        public IActionResult GetTransactionHistory(int customerId)
        {
            return Ok(_service.GetTransactionHistory(customerId));
        }

        [HttpGet("levels")]
        public IActionResult GetMembershipLevels()
        {
            return Ok(_service.GetAllMembershipLevels());
        }

        [HttpPost("add-points")]
        public IActionResult AddPoints([FromBody] AddPointsDto dto)
        {
            try
            {
                var result = _service.AddPoints(dto.CustomerId, dto.Points, dto.Source, dto.Description);
                return Ok(new { message = "Points added successfully", transaction = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("redeem-points")]
        public IActionResult RedeemPoints([FromBody] RedeemPointsDto dto)
        {
            try
            {
                var result = _service.RedeemPoints(dto.CustomerId, dto.Points, dto.Description);
                return Ok(new { message = "Points redeemed successfully", transaction = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            return Ok(_service.GetLoyaltyStatistics());
        }
    }
}