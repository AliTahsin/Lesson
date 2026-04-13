using Microsoft.AspNetCore.Mvc;
using CRM_Loyalty.API.Services;
using CRM_Loyalty.API.DTOs;

namespace CRM_Loyalty.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreferencesController : ControllerBase
    {
        private readonly LoyaltyService _service;

        public PreferencesController()
        {
            _service = new LoyaltyService();
        }

        [HttpGet("{customerId}")]
        public IActionResult GetByCustomer(int customerId)
        {
            return Ok(_service.GetCustomerPreferences(customerId));
        }

        [HttpPost("{customerId}")]
        public IActionResult AddPreference(int customerId, [FromBody] PreferenceDto dto)
        {
            var result = _service.AddPreference(customerId, dto);
            return Ok(new { message = "Preference added successfully", preference = result });
        }

        [HttpDelete("{preferenceId}")]
        public IActionResult DeletePreference(int preferenceId)
        {
            if (_service.DeletePreference(preferenceId))
                return Ok(new { message = "Preference deleted successfully" });
            return NotFound();
        }
    }
}