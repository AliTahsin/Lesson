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
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public RolesController(IRoleRepository roleRepository, IPermissionRepository permissionRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleRepository.GetAllAsync();
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);
            
            foreach (var roleDto in roleDtos)
            {
                var permissions = await _roleRepository.GetRolePermissionsAsync(roleDto.Id);
                roleDto.Permissions = permissions.Select(p => p.Code).ToList();
            }
            
            return Ok(roleDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return NotFound();
            
            var roleDto = _mapper.Map<RoleDto>(role);
            var permissions = await _roleRepository.GetRolePermissionsAsync(id);
            roleDto.Permissions = permissions.Select(p => p.Code).ToList();
            
            return Ok(roleDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            role.IsActive = true;
            role.CreatedAt = DateTime.Now;
            
            await _roleRepository.AddAsync(role);
            return Ok(new { message = "Role created successfully", role });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto dto)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return NotFound();
            
            _mapper.Map(dto, role);
            role.UpdatedAt = DateTime.Now;
            await _roleRepository.UpdateAsync(role);
            
            return Ok(new { message = "Role updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleRepository.DeleteAsync(id);
            if (!result)
                return NotFound();
            
            return Ok(new { message = "Role deleted successfully" });
        }

        [HttpPost("{id}/permissions/{permissionId}")]
        public async Task<IActionResult> AddPermission(int id, int permissionId)
        {
            var result = await _roleRepository.AddPermissionToRoleAsync(id, permissionId);
            if (!result)
                return NotFound();
            
            return Ok(new { message = "Permission added to role successfully" });
        }

        [HttpDelete("{id}/permissions/{permissionId}")]
        public async Task<IActionResult> RemovePermission(int id, int permissionId)
        {
            var result = await _roleRepository.RemovePermissionFromRoleAsync(id, permissionId);
            if (!result)
                return NotFound();
            
            return Ok(new { message = "Permission removed from role successfully" });
        }
    }
}