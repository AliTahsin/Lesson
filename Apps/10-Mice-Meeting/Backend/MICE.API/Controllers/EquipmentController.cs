using Microsoft.AspNetCore.Mvc;
using MICE.API.DTOs;
using MICE.API.Services;

namespace MICE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var equipment = await _equipmentService.GetEquipmentByIdAsync(id);
            if (equipment == null) return NotFound();
            return Ok(equipment);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var equipment = await _equipmentService.GetEquipmentByHotelAsync(hotelId);
            return Ok(equipment);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var equipment = await _equipmentService.GetEquipmentByCategoryAsync(category);
            return Ok(equipment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEquipmentDto dto)
        {
            var equipment = await _equipmentService.CreateEquipmentAsync(dto);
            return Ok(equipment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEquipmentDto dto)
        {
            try
            {
                var equipment = await _equipmentService.UpdateEquipmentAsync(id, dto);
                return Ok(equipment);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromQuery] int quantity)
        {
            var equipment = await _equipmentService.UpdateStockAsync(id, quantity);
            return Ok(equipment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _equipmentService.DeleteEquipmentAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Equipment deleted successfully" });
        }
    }
}