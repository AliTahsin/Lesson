using Microsoft.AspNetCore.Mvc;
using CRM_Loyalty.API.Services;
using CRM_Loyalty.API.DTOs;

namespace CRM_Loyalty.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly LoyaltyService _service;

        public CustomersController()
        {
            _service = new LoyaltyService();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAllCustomers());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _service.GetCustomerById(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpGet("email/{email}")]
        public IActionResult GetByEmail(string email)
        {
            var customer = _service.GetCustomerByEmail(email);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpGet("number/{customerNumber}")]
        public IActionResult GetByNumber(string customerNumber)
        {
            var customer = _service.GetCustomerByNumber(customerNumber);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerDto dto)
        {
            try
            {
                var customerDto = new CustomerDto
                {
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                    Country = dto.Country,
                    City = dto.City,
                    PreferredLanguage = dto.PreferredLanguage
                };
                var result = _service.CreateCustomer(customerDto);
                return Ok(new { message = "Customer created successfully", customer = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CustomerDto dto)
        {
            var result = _service.UpdateCustomer(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}