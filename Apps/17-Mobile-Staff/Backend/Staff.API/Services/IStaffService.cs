using Staff.API.DTOs;

namespace Staff.API.Services
{
    public interface IStaffService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<StaffDto> GetStaffByIdAsync(int id);
        Task<List<StaffDto>> GetStaffByHotelAsync(int hotelId);
        Task<StaffDto> UpdateProfileAsync(int id, UpdateStaffDto dto);
        Task<bool> ChangePasswordAsync(int id, ChangePasswordDto dto);
        Task<bool> LogoutAsync(int id);
    }
}