using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Auth.API.DTOs;
using Auth.API.Repositories;
using Auth.API.Security;
using AutoMapper;

namespace Auth.API.Controllers
{
    [Authorize(Policy = Policies.CanManageUsers)]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionsController(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var permissions = await _permissionRepository.GetAllAsync();
            var permissionDtos = _mapper.Map<List<PermissionDto>>(permissions);
            return Ok(permissionDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null)
                return NotFound();
            
            var permissionDto = _mapper.Map<PermissionDto>(permission);
            return Ok(permissionDto);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var permissions = await _permissionRepository.GetByCategoryAsync(category);
            var permissionDtos = _mapper.Map<List<PermissionDto>>(permissions);
            return Ok(permissionDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionDto dto)
        {
            var permission = _mapper.Map<Permission>(dto);
            permission.IsActive = true;
            permission.CreatedAt = DateTime.Now;
            
            await _permissionRepository.AddAsync(permission);
            return Ok(new { message = "Permission created successfully", permission });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PermissionDto dto)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null)
                return NotFound();
            
            _mapper.Map(dto, permission);
            await _permissionRepository.UpdateAsync(permission);
            
            return Ok(new { message = "Permission updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _permissionRepository.DeleteAsync(id);
            if (!result)
                return NotFound();
            
            return Ok(new { message = "Permission deleted successfully" });
        }
    }
}