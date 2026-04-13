using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Auth.API.DTOs;
using Auth.API.Repositories;
using Auth.API.Security;
using AutoMapper;

namespace Auth.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [Authorize(Policy = Policies.CanManageUsers)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            
            foreach (var userDto in userDtos)
            {
                var user = users.First(u => u.Id == userDto.Id);
                var roles = user.RoleIds.Select(async id => (await _roleRepository.GetByIdAsync(id))?.Name).Select(t => t.Result).Where(r => r != null).ToList();
                userDto.Roles = roles;
            }
            
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var currentUserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var isAdmin = User.HasClaim("permission", "user:manage");
            
            if (id != currentUserId && !isAdmin)
                return Forbid();
            
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            
            var userDto = _mapper.Map<UserDto>(user);
            var roles = user.RoleIds.Select(async rid => (await _roleRepository.GetByIdAsync(rid))?.Name).Select(t => t.Result).Where(r => r != null).ToList();
            userDto.Roles = roles;
            
            return Ok(userDto);
        }

        [HttpGet("hotel/{hotelId}")]
        [Authorize(Policy = Policies.HotelManager)]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var users = await _userRepository.GetByHotelAsync(hotelId);
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var currentUserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var isAdmin = User.HasClaim("permission", "user:manage");
            
            if (id != currentUserId && !isAdmin)
                return Forbid();
            
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            
            _mapper.Map(dto, user);
            await _userRepository.UpdateAsync(user);
            
            return Ok(new { message = "User updated successfully" });
        }

        [Authorize(Policy = Policies.CanManageUsers)]
        [HttpPut("{id}/roles")]
        public async Task<IActionResult> UpdateRoles(int id, [FromBody] UpdateUserRolesDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            
            user.RoleIds = dto.RoleIds;
            await _userRepository.UpdateAsync(user);
            
            return Ok(new { message = "User roles updated successfully" });
        }

        [Authorize(Policy = Policies.CanManageUsers)]
        [HttpPost("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            
            user.IsActive = true;
            user.LockoutEnd = null;
            user.FailedLoginAttempts = 0;
            await _userRepository.UpdateAsync(user);
            
            return Ok(new { message = "User activated successfully" });
        }

        [Authorize(Policy = Policies.CanManageUsers)]
        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            
            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
            
            return Ok(new { message = "User deactivated successfully" });
        }

        [Authorize(Policy = Policies.CanManageUsers)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if (!result)
                return NotFound();
            
            return Ok(new { message = "User deleted successfully" });
        }
    }
}