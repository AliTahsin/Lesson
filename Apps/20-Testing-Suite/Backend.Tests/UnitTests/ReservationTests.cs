using Xunit;
using Moq;
using FluentAssertions;
using ReservationSystem.API.Services;
using ReservationSystem.API.DTOs;

namespace Backend.Tests.UnitTests
{
    public class ReservationTests : TestBase
    {
        private readonly ReservationService _service;

        public ReservationTests()
        {
            _service = new ReservationService();
        }

        [Fact]
        public void GetAllReservations_ShouldReturnAllReservations()
        {
            // Act
            var result = _service.GetAllReservations();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public void GetReservationById_WithValidId_ShouldReturnReservation()
        {
            // Arrange
            var validId = 1;

            // Act
            var result = _service.GetReservationById(validId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(validId);
        }

        [Fact]
        public void GetTodayArrivals_ShouldReturnCorrectType()
        {
            // Act
            var result = _service.GetTodayArrivals();

            // Assert
            result.Should().NotBeNull();
            result.Should().AllBeOfType<ReservationResponseDto>();
        }

        [Theory]
        [InlineData("ahmet.yilmaz@email.com", 2)]
        [InlineData("nonexistent@email.com", 0)]
        public void GetReservationsByGuestEmail_ShouldReturnCorrectCount(string email, int expectedCount)
        {
            // Act
            var result = _service.GetReservationsByGuestEmail(email);

            // Assert
            result.Should().HaveCount(expectedCount);
        }

        [Fact]
        public void CreateReservation_WithInvalidDates_ShouldThrowException()
        {
            // Arrange
            var dto = new CreateReservationDto
            {
                CheckInDate = DateTime.Now.AddDays(5),
                CheckOutDate = DateTime.Now.AddDays(3), // Invalid: check-out before check-in
                GuestFirstName = "Test",
                GuestLastName = "User",
                GuestEmail = "test@email.com"
            };

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _service.CreateReservation(dto));
        }
    }
}