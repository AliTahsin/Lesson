using Xunit;
using Moq;
using FluentAssertions;
using Auth.API.Services;
using Auth.API.DTOs;

namespace Backend.Tests.UnitTests
{
    public class AuthTests : TestBase
    {
        private readonly AuthService _service;
        private readonly Mock<IUserRepository> _mockUserRepository;

        public AuthTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _service = new AuthService();
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldSucceed()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                EmailOrUsername = "admin@hotel.com",
                Password = "Admin123!"
            };

            // Act
            var result = _service.LoginAsync(loginDto).Result;

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Login_WithInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                EmailOrUsername = "invalid@hotel.com",
                Password = "wrongpassword"
            };

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _service.LoginAsync(loginDto));
        }

        [Fact]
        public void Register_WithNewUser_ShouldSucceed()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "testnew@hotel.com",
                PhoneNumber = "+905551234567",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            };

            // Act
            var result = _service.RegisterAsync(registerDto).Result;

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(registerDto.Email);
        }

        [Theory]
        [InlineData("short", "short")]
        [InlineData("password123", "different")]
        public void Register_WithInvalidPassword_ShouldThrowException(string password, string confirmPassword)
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@hotel.com",
                PhoneNumber = "+905551234567",
                Password = password,
                ConfirmPassword = confirmPassword
            };

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _service.RegisterAsync(registerDto));
        }
    }
}