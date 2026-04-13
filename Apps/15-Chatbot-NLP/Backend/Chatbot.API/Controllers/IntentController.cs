using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chatbot.API.DTOs;
using Chatbot.API.Repositories;
using AutoMapper;

namespace Chatbot.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class IntentController : ControllerBase
    {
        private readonly IIntentRepository _intentRepository;
        private readonly IMapper _mapper;

        public IntentController(IIntentRepository intentRepository, IMapper mapper)
        {
            _intentRepository = intentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var intents = await _intentRepository.GetAllAsync();
            var intentDtos = _mapper.Map<List<IntentDto>>(intents);
            return Ok(intentDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var intent = await _intentRepository.GetByIdAsync(id);
            if (intent == null) return NotFound();
            var intentDto = _mapper.Map<IntentDto>(intent);
            return Ok(intentDto);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var intents = await _intentRepository.GetActiveIntentsAsync();
            var intentDtos = _mapper.Map<List<IntentDto>>(intents);
            return Ok(intentDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIntentDto dto)
        {
            var intent = _mapper.Map<Intent>(dto);
            intent.IsActive = true;
            intent.CreatedAt = DateTime.Now;
            
            await _intentRepository.AddAsync(intent);
            return Ok(new { message = "Intent created successfully", intent });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateIntentDto dto)
        {
            var intent = await _intentRepository.GetByIdAsync(id);
            if (intent == null) return NotFound();
            
            _mapper.Map(dto, intent);
            intent.UpdatedAt = DateTime.Now;
            
            await _intentRepository.UpdateAsync(intent);
            return Ok(new { message = "Intent updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _intentRepository.DeleteAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Intent deleted successfully" });
        }
    }
}