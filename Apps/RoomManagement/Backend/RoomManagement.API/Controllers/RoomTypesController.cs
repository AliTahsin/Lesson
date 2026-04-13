using Microsoft.AspNetCore.Mvc;
using RoomManagement.API.Services;

namespace RoomManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypesController : ControllerBase
    {
        private readonly RoomManagementService _service;

        public RoomTypesController()
        {
            _service = new RoomManagementService();
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAllRoomTypes());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var roomType = _service.GetRoomTypeById(id);
            if (roomType == null) return NotFound();
            return Ok(roomType);
        }
    }
}