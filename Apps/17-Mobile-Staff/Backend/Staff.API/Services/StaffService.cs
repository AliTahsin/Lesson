using AutoMapper;
using Staff.API.Models;
using Staff.API.DTOs;
using Staff.API.Repositories;
using Staff.API.Security;

namespace Staff.API.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly ILogger<StaffService> _logger;

        public StaffService(
            IStaffRepository staffRepository,
            ITokenService tokenService,
            IPasswordHasher passwordHasher,
            IMapper mapper,
            ILogger<StaffService> logger)
        {
            _staffRepository = staffRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var staff = await _staffRepository.GetByEmailAsync(loginDto.Email);
            if (staff == null)
                throw new Exception("Invalid email or password");

            if (!_passwordHasher.Verify(loginDto.Password, staff.PasswordHash))
                throw new Exception("Invalid email or password");

            if (!staff.IsActive)
                throw new Exception("Account is deactivated");

            var token = _tokenService.GenerateToken(staff);
            await _staffRepository.UpdateLastLoginAsync(staff.Id);

            return new LoginResponseDto
            {
                AccessToken = token,
                Staff = _mapper.Map<StaffDto>(staff),
                Role = staff.Role
            };
        }

        public async Task<StaffDto> GetStaffByIdAsync(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            return staff != null ? _mapper.Map<StaffDto>(staff) : null;
        }

        public async Task<List<StaffDto>> GetStaffByHotelAsync(int hotelId)
        {
            var staff = await _staffRepository.GetByHotelAsync(hotelId);
            return _mapper.Map<List<StaffDto>>(staff);
        }

        public async Task<StaffDto> UpdateProfileAsync(int id, UpdateStaffDto dto)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
                throw new Exception("Staff not found");

            _mapper.Map(dto, staff);
            await _staffRepository.UpdateAsync(staff);
            return _mapper.Map<StaffDto>(staff);
        }

        public async Task<bool> ChangePasswordAsync(int id, ChangePasswordDto dto)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
                throw new Exception("Staff not found");

            if (!_passwordHasher.Verify(dto.CurrentPassword, staff.PasswordHash))
                throw new Exception("Current password is incorrect");

            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("New passwords do not match");

            staff.PasswordHash = _passwordHasher.Hash(dto.NewPassword);
            await _staffRepository.UpdateAsync(staff);
            return true;
        }

        public async Task<bool> LogoutAsync(int id)
        {
            return await Task.FromResult(true);
        }
    }
}