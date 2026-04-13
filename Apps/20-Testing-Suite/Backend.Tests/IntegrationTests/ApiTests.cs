using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Backend.Tests.IntegrationTests
{
    public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetHotelsEndpoint_ShouldReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/hotels");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetReservationsEndpoint_ShouldReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/reservations");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task PostPaymentEndpoint_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var paymentData = new
            {
                Amount = 100,
                Currency = "EUR",
                CardNumber = "4111111111111111",
                CardHolderName = "Test User",
                ExpiryMonth = 12,
                ExpiryYear = 2025,
                Cvv = "123"
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(paymentData),
                System.Text.Encoding.UTF8,
                "application/json");

            // Act
            var response = await client.PostAsync("/api/payments/process", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}