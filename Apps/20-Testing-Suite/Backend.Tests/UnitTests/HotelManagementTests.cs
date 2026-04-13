using Xunit;
using Moq;
using FluentAssertions;
using HotelManagement.API.Services;
using HotelManagement.API.Repositories;
using HotelManagement.API.Models;

namespace Backend.Tests.UnitTests
{
    public class HotelManagementTests : TestBase
    {
        private readonly HotelManagementService _service;
        private readonly Mock<IHotelRepository> _mockRepository;

        public HotelManagementTests()
        {
            _mockRepository = new Mock<IHotelRepository>();
            _service = new HotelManagementService();
        }

        [Fact]
        public void GetAllHotels_ShouldReturnAllHotels()
        {
            // Act
            var result = _service.GetAllHotels();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public void GetHotelById_WithValidId_ShouldReturnHotel()
        {
            // Arrange
            var validId = 1;

            // Act
            var result = _service.GetHotelById(validId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(validId);
        }

        [Fact]
        public void GetHotelById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = 9999;

            // Act
            var result = _service.GetHotelById(invalidId);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("İstanbul", 2)]
        [InlineData("Ankara", 1)]
        [InlineData("İzmir", 1)]
        public void GetHotelsByCity_ShouldReturnCorrectCount(string city, int expectedCount)
        {
            // Act
            var result = _service.GetHotelsByCity(city);

            // Assert
            result.Should().HaveCount(expectedCount);
        }

        [Fact]
        public void AddHotel_ShouldIncreaseHotelCount()
        {
            // Arrange
            var newHotel = new Hotel
            {
                Name = "Test Hotel",
                City = "Test City",
                Country = "Test Country",
                StarRating = 4
            };

            var initialCount = _service.GetAllHotels().Count;

            // Act
            var added = _service.AddHotel(newHotel);
            var newCount = _service.GetAllHotels().Count;

            // Assert
            added.Should().NotBeNull();
            newCount.Should().Be(initialCount + 1);
        }
    }
}