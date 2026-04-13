using AutoMapper;
using Auth.API.Models;
using Auth.API.DTOs;
using Auth.API.Repositories;
using Auth.API.Security;

namespace Auth.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenService _tokenService;
        private readonly ITwoFactorService _twoFactorService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ITokenService tokenService,
            ITwoFactorService twoFactorService,
            IPasswordHasher passwordHasher,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
            _twoFactorService = twoFactorService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Find user by email or username
            var user = await _userRepository.GetByEmailAsync(loginDto.EmailOrUsername) ??
                       await _userRepository.GetByUsernameAsync(loginDto.EmailOrUsername);

            if (user == null)
                throw new Exception("Invalid email/username or password");

            // Check if user is locked
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.Now)
                throw new Exception($"Account is locked. Try again after {user.LockoutEnd.Value:HH:mm}");

            // Verify password
            if (!_passwordHasher.Verify(loginDto.Password, user.PasswordHash))
            {
                user.FailedLoginAttempts++;
                await _userRepository.UpdateFailedLoginAttemptsAsync(user.Id, user.FailedLoginAttempts);
                
                if (user.FailedLoginAttempts >= 5)
                {
                    await _userRepository.LockUserAsync(user.Id, DateTime.Now.AddMinutes(15));
                    throw new Exception("Account has been locked due to too many failed attempts");
                }
                
                throw new Exception("Invalid email/username or password");
            }

            // Reset failed attempts on successful login
            if (user.FailedLoginAttempts > 0)
                await _userRepository.UpdateFailedLoginAttemptsAsync(user.Id, 0);

            // Check if 2FA is enabled
            if (user.TwoFactorEnabled)
            {
                // Generate and send 2FA code
                await _twoFactorService.GenerateAndSendCodeAsync(user.Id, user.TwoFactorMethod);
                
                return new LoginResponseDto
                {
                    RequiresTwoFactor = true,
                    UserId = user.Id,
                    Message = $"2FA code sent to your {user.TwoFactorMethod}"
                };
            }

            // Generate tokens
            var permissions = await GetUserPermissionsAsync(user);
            var accessToken = _tokenService.GenerateAccessToken(user, permissions);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id, loginDto.DeviceId, loginDto.DeviceName, loginDto.IpAddress, loginDto.UserAgent);

            await _userRepository.UpdateLastLoginAsync(user.Id);

            return new LoginResponseDto
            {
                RequiresTwoFactor = false,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserDto>(user),
                Message = "Login successful"
            };
        }

        public async Task<LoginResponseDto> LoginWith2FAAsync(TwoFactorVerifyDto dto)
        {
            var isValid = await _twoFactorService.VerifyCodeAsync(dto.UserId, dto.Code);
            if (!isValid)
                throw new Exception("Invalid 2FA code");

            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new Exception("User not found");

            var permissions = await GetUserPermissionsAsync(user);
            var accessToken = _tokenService.GenerateAccessToken(user, permissions);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id, dto.DeviceId, dto.DeviceName, dto.IpAddress, dto.UserAgent);

            await _userRepository.UpdateLastLoginAsync(user.Id);

            return new LoginResponseDto
            {
                RequiresTwoFactor = false,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserDto>(user),
                Message = "Login successful"
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if user exists
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists");

            // Create new user
            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Username = registerDto.Username,
                PasswordHash = _passwordHasher.Hash(registerDto.Password),
                IsActive = true,
                HotelId = registerDto.HotelId,
                Department = registerDto.Department,
                Position = registerDto.Position,
                RoleIds = new List<int> { GetDefaultRoleId() },
                CreatedAt = DateTime.Now
            };

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<TokenDto> RefreshTokenAsync(string refreshToken)
        {
            var (userId, token) = await _tokenService.ValidateRefreshTokenAsync(refreshToken);
            if (userId == null)
                throw new Exception("Invalid refresh token");

            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
                throw new Exception("User not found");

            var permissions = await GetUserPermissionsAsync(user);
            var newAccessToken = _tokenService.GenerateAccessToken(user, permissions);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user.Id, token.DeviceId, token.DeviceName, token.IpAddress, token.UserAgent);

            // Revoke old refresh token
            await _tokenService.RevokeRefreshTokenAsync(refreshToken);

            return new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> LogoutAsync(int userId, string refreshToken)
        {
            await _tokenService.RevokeRefreshTokenAsync(refreshToken);
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            if (!_passwordHasher.Verify(dto.CurrentPassword, user.PasswordHash))
                throw new Exception("Current password is incorrect");

            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("New password and confirmation do not match");

            var newPasswordHash = _passwordHasher.Hash(dto.NewPassword);
            await _userRepository.ChangePasswordAsync(userId, newPasswordHash);

            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return true; // Don't reveal that user doesn't exist

            // Generate reset token and send email
            var resetToken = Guid.NewGuid().ToString();
            // TODO: Send email with reset link
            _logger.LogInformation($"Password reset token for {email}: {resetToken}");

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            // TODO: Validate reset token
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("User not found");

            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            var newPasswordHash = _passwordHasher.Hash(dto.NewPassword);
            await _userRepository.ChangePasswordAsync(user.Id, newPasswordHash);

            return true;
        }

        public async Task<bool> EnableTwoFactorAsync(int userId, string method)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            await _userRepository.UpdateTwoFactorSettingsAsync(userId, true, method);
            return true;
        }

        public async Task<bool> DisableTwoFactorAsync(int userId)
        {
            await _userRepository.UpdateTwoFactorSettingsAsync(userId, false, null);
            return true;
        }

        public async Task<string> SendTwoFactorCodeAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            return await _twoFactorService.GenerateAndSendCodeAsync(userId, user.TwoFactorMethod);
        }

        public async Task<bool> VerifyTwoFactorCodeAsync(int userId, string code)
        {
            return await _twoFactorService.VerifyCodeAsync(userId, code);
        }

        private async Task<List<string>> GetUserPermissionsAsync(User user)
        {
            var permissions = new List<string>();
            foreach (var roleId in user.RoleIds)
            {
                var rolePermissions = await _roleRepository.GetRolePermissionsAsync(roleId);
                permissions.AddRange(rolePermissions.Select(p => p.Code));
            }
            return permissions.Distinct().ToList();
        }

        private int GetDefaultRoleId()
        {
            var defaultRole = _roleRepository.GetAllAsync().Result.FirstOrDefault(r => r.IsDefault);
            return defaultRole?.Id ?? 4; // Guest role
        }
    }
}